using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHP,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightningDamage,
    cooldownDecreasePercentage,
}
public enum AttackType{
    PhysicalDmg,
    CritDmg,
    MagicalDmg,
    FireDmg,
    IceDmg,
    LightingDmg,
}
public class CharacterStats : MonoBehaviour
{
    [Header("主要属性")]
    public Stat strength;  //damage + 1; crit_power + 1%
    public Stat agility;  //evasion + 1%; crit_chance + 1%
    public Stat intelligence; //magic_damage + 1; magic_resistance + 3
    public Stat vitality; //maxHP + 5

    [Header("防御属性")]
    public Stat maxHP;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("攻击属性")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;  //critPower = 150% by default

    [Header("魔法属性")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    [Header("冷却时间减少百分比")]
    public Stat cooldownDecreasePercentage;

    [Header("异常状态信息")]
    public bool isOnFire; //持续伤害
    public bool isOnIce; //护甲减少20%
    public bool isOnLighting; //命中率减少20% (敌人闪避率增加20%)
    public Stat igniteDuration;
    public Stat chillDuration;
    public Stat shockDuration;
    [SerializeField] private GameObject thunderStrikePrefab;
    
    private int thunderStrikeDamage;

    //无敌检测在DoDamage()中，TakeDamage()中没有无敌检测
    public bool isInvincible { get; private set; }

    [Space]
    public float currentHP;

    //参见ApplyAilment()
    private float fireElementTimer;
    private float iceElementTimer;
    private float lightingElementTimer;

    private float ignitedDamageCooldown = 0.3f;
    private float fireDamageTimer;
    private int igniteDamage; //在DoMagicDamage()中设置

    //脆弱状态将承受额外10%的伤害
    public bool isVulnerable;
    public bool isDead;

    //属性计算:
    //total_evasion = evasion + agility + [攻击者雷击效果];
    //total_damage = (damage + strength) * [total_crit_power] - target.armor * [target chill effect];
    //total_crit_chance = critChance + agility;
    //total_crit_power = critPower + strength;
    //total_magic_damage = (fireDamage + iceDamage + lightningDamage + intelligence) - (target.magicResistance + 3 * target.intelligence);

    public System.Action onHealthChanged;

    //为了使实体的HP条在开始时正确更新
    //如果不希望使用此bool变量
    //可以在Unity项目设置中更改脚本执行顺序
    //顺序: CharacterStats -> HPBar_UI

    protected EntityFX fx;

    protected virtual void Awake()
    {
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Start()
    {
        currentHP = getMaxHP();
        critPower.SetDefaultValue(150);

        //HPBarCanBeInitialized = true;
    }

    protected virtual void Update()
    {
        float deltaTime = Time.deltaTime;

        fireElementTimer -= deltaTime;
        iceElementTimer -= deltaTime;
        lightingElementTimer -= deltaTime;

        fireDamageTimer -= deltaTime;

        if (fireElementTimer < 0){
            isOnFire = false;
        }
        if (iceElementTimer < 0){
            isOnIce = false;}
        if (lightingElementTimer < 0){
            isOnLighting = false;
        }
        if (isOnFire){
            DealIgniteDamage();
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if(_targetStats == null)
        {
            Destroy(gameObject);
            return;
        }
        if (_targetStats.isInvincible)
        {
            return;
        }
        if (TargetCanEvadeThisAttack(_targetStats))
        {
            return;
        }
        float _totalDamage = damage.GetValue() + strength.GetValue();

        AttackType crit = CanCrit();

        if (crit == AttackType.CritDmg)
        {
            GameManager.instance.consule_UI.AddDebug("<玩家>:"+ name + "暴击！");
            _totalDamage = CalculatCritDamage(_totalDamage);
        }

        fx.CreateHitFX(_targetStats.transform, crit);

        _totalDamage = CheckTargetArmor(_targetStats, _totalDamage);

        _totalDamage = CheckTargetVulnerability(_targetStats, _totalDamage);

        _targetStats.TakeDamage(_totalDamage, transform, _targetStats.transform, crit);

        DoMagicDamage(_targetStats, transform);
    }

    public virtual void TakeDamage(float _damage, Transform _attacker, Transform _attackee, AttackType attackType)
    {
        //如果实体处于无敌状态，则不会受到伤害
        if (isInvincible)
        {
            return;
        }
        DecreaseHPBy(_damage, attackType);

        //GameManager.instance.consule_UI.AddDebug($"{gameObject.name} received {_damage} damage");

        _attackee.GetComponent<Entity>()?.DamageFlashEffect();
        _attackee.GetComponent<Entity>()?.DamageKnockbackEffect(_attacker, _attackee);

        if (currentHP <= 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        GameManager.instance.consule_UI.AddDebug($"<系统>:{gameObject.name} 被杀死了");
    }

    public virtual void DieFromFalling()
    {
        if (isDead)
        {
            return;
        }
        currentHP = 0;
        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
        Die();
    }

    public void BecomeInvincible(bool _invincible)
    {
        isInvincible = _invincible;
    }

    #region 魔法和异常状态
    public virtual void DoMagicDamage(CharacterStats _targetStats, Transform _attacker)
    {
        float _fireDamage = fireDamage.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _lightningDamage = lightningDamage.GetValue();

        float _totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        _totalMagicDamage = CheckTargetMagicResistance(_targetStats, _totalMagicDamage);

        _targetStats.TakeDamage(_totalMagicDamage, _attacker, _targetStats.transform, AttackType.MagicalDmg);

        //只有当至少一种魔法伤害大于0时，才能施加异常状态
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    private void AttemptToApplyAilments(CharacterStats _targetStats, float _fireDamage, float _iceDamage, float _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        if (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            ApplyRandomAilment(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
            return;
        }

        if (canApplyIgnite)
        {
            ApplyIgniteDamage(_targetStats, _fireDamage);
        }

        if (canApplyShock)
        {
            ApplyShockDamage(_targetStats, _lightningDamage);
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private void ApplyRandomAilment(CharacterStats _targetStats, float _fireDamage, float _iceDamage, float _lightningDamage)
    {
        var applicableAilments = new List<(float chance, System.Action action)>();
        if (_fireDamage > 0) applicableAilments.Add((0.3f, () => ApplyIgniteDamage(_targetStats, _fireDamage)));
        if (_iceDamage > 0) applicableAilments.Add((0.5f, () => ApplyChillDamage(_targetStats, _iceDamage)));
        if (_lightningDamage > 0) applicableAilments.Add((0.5f, () => ApplyShockDamage(_targetStats, _lightningDamage)));

        var selectedAilment = applicableAilments.Where(a => Random.value < a.chance).OrderBy(a => Random.value).FirstOrDefault();
        selectedAilment.action?.Invoke();
    }

    private void ApplyIgniteDamage(CharacterStats _targetStats, float _fireDamage)
    {
        _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
    }

    private void ApplyChillDamage(CharacterStats _targetStats, float _iceDamage)
    {
        _targetStats.ApplyAilments(true, false, false);
    }

    private void ApplyShockDamage(CharacterStats _targetStats, float _lightningDamage)
    {
        _targetStats.SetupThunderStrikeDamage(Mathf.RoundToInt(_lightningDamage * 0.2f));
        _targetStats.ApplyAilments(false, false, true);
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (_ignite && !isOnFire && !isOnIce && !isOnLighting)
        {
            isOnFire = true;
            fireElementTimer = igniteDuration.GetValue();
            StartCoroutine(fx.EnableIgniteFXForTime_Coroutine(fireElementTimer));
            GameManager.instance.consule_UI.AddDebug($"<系统>:{gameObject.name} 被点燃了");
        }

        if (_chill && !isOnFire && !isOnIce && !isOnLighting)
        {
            isOnIce = true;
            iceElementTimer = chillDuration.GetValue();
            StartCoroutine(fx.EnableChillFXForTime_Coroutine(iceElementTimer));
            float _slowPercentage = 0.2f;
            GetComponent<Entity>()?.SlowSpeedBy(_slowPercentage, chillDuration.GetValue());
            GameManager.instance.consule_UI.AddDebug($"<系统>:{gameObject.name} 被冰冻了");
        }

        if (_shock && !isOnFire && !isOnIce)
        {
            ApplyShockAilment(_shock);
            GameManager.instance.consule_UI.AddDebug($"<系统>:{gameObject.name} 被雷击了");
        }
    }

    public void ApplyShockAilment(bool _shock)
    {
        //不能对已经处于雷击异常状态的敌人施加雷击异常状态
        if (isOnLighting)
        {
            return;
        }

        isOnLighting = _shock;
        lightingElementTimer = shockDuration.GetValue();

        //fx.EnableShockFXForTime(shockedAilmentTimer);
        StartCoroutine(fx.EnableShockFXForTime_Coroutine(lightingElementTimer));
    }

    private void GenerateThunderStrikeAndHitClosestEnemy(float _targetScanRadius)
    {
        //找到最近的目标
        //实例化雷霆一击
        //设置雷霆一击
        Transform closestEnemy = null;

        //找到搜索半径内的所有敌人
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _targetScanRadius);

        float closestDistanceToEnemy = Mathf.Infinity;

        //找到最近的敌人
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float currentDistanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (currentDistanceToEnemy < closestDistanceToEnemy)
                {
                    closestDistanceToEnemy = currentDistanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        //如果找不到最近的敌人,
        //敌人自己就是最近的敌人
        if (closestEnemy == null)
        {
            closestEnemy = transform;
        }

        //生成雷霆一击并攻击最近的敌人
        if (closestEnemy != null)
        {
            GameObject newThunderStrike = Instantiate(thunderStrikePrefab, transform.position, Quaternion.identity);

            newThunderStrike.GetComponent<Skill_ThunderStrikeController>()?.Setup(thunderStrikeDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    private void DealIgniteDamage()
    {
        if (fireDamageTimer < 0)
        {
            GameManager.instance.consule_UI.AddDebug($"<系统>:火焰伤害+{igniteDamage}");
            DecreaseHPBy(igniteDamage, AttackType.FireDmg);

            if (currentHP <= 0 && !isDead)
            {
                Die();
            }

            fireDamageTimer = ignitedDamageCooldown;
        }
    }

    public void SetupIgniteDamage(int _igniteDamage)
    {
        igniteDamage = _igniteDamage;
    }

    public void SetupThunderStrikeDamage(int _thunderStrikeDamage)
    {
        thunderStrikeDamage = _thunderStrikeDamage;
    }
    #endregion

    #region HP和伤害计算 - 护甲, 暴击, 魔法抗性, 脆弱, 闪避
    protected float CheckTargetArmor(CharacterStats _targetStats, float _totalDamage)
    {
        float armorReduction = _targetStats.armor.GetValue() * (_targetStats.isOnIce ? 0.8f : 1f);
        _totalDamage = Mathf.Max(0, _totalDamage - Mathf.RoundToInt(armorReduction));
        return _totalDamage;
    }

    private float CheckTargetMagicResistance(CharacterStats _targetStats, float _totalMagicDamage)
    {
        _totalMagicDamage -= _targetStats.magicResistance.GetValue() + (3 * _targetStats.intelligence.GetValue());

        _totalMagicDamage = Mathf.Clamp(_totalMagicDamage, 0, int.MaxValue);
        return _totalMagicDamage;
    }

    protected bool TargetCanEvadeThisAttack(CharacterStats _targetStats)
    {
        float _totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isOnLighting)
        {
            _totalEvasion += 20;
        }

        if (Random.Range(0, 100) < _totalEvasion)
        {
            GameManager.instance.consule_UI.AddDebug("<系统>:闪避伤害");
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    protected AttackType CanCrit()
    {
        float _totalCritChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= _totalCritChance)
        {
            return AttackType.CritDmg;
        }

        return AttackType.PhysicalDmg;
    }

    protected float CalculatCritDamage(float _damage)
    {
        float _totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float critDamage = _damage * _totalCritPower;

        return critDamage;
    }

    public float CheckTargetVulnerability(CharacterStats _targetStats, float _totalDamage)
    {
        if (_targetStats.isVulnerable)
        {
            _totalDamage = Mathf.RoundToInt(_totalDamage * 1.1f);
        }

        return _totalDamage;
    }

    public virtual void OnEvasion()
    {

    }

    #region HP
    public virtual float DecreaseHPBy(float _takenDamage, AttackType attackType)
    {
        currentHP -= _takenDamage;

        if (_takenDamage > 0)
        {
            GameObject popUpText = fx.CreatePopUpText(_takenDamage.ToString());
            Color  color;
            
            switch (attackType)
            {
                case AttackType.PhysicalDmg: color = Color.white; break;
                case AttackType.FireDmg: color = Color.red; break;
                case AttackType.IceDmg: color = Color.blue; break;
                case AttackType.LightingDmg: color = Color.yellow; break;
                case AttackType.CritDmg: color = Color.yellow; break;
                case AttackType.MagicalDmg: color = Color.cyan; break;
                default: color = Color.white; break;
            }
            popUpText.GetComponent<TextMeshPro>().color = color;
        }

        GameManager.instance.consule_UI.AddDebug($"<系统>:{gameObject.name} 收到了 {_takenDamage} 伤害");

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }

        return _takenDamage;
    }

    public virtual void IncreaseHPBy(int _HP)
    {
        currentHP += _HP;

        if (currentHP > getMaxHP())
        {
            currentHP = getMaxHP();
        }

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }
    #endregion

    #endregion

    public void BecomeVulnerableForTime(float _seconds)
    {
        StartCoroutine(BecomeVulnerableForTime_Coroutine(_seconds));
    }

    private IEnumerator BecomeVulnerableForTime_Coroutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }

    public virtual void IncreaseStatByTime(Stat _statToModify, float _modifier, float _duration)
    {
        StartCoroutine(StatModify_Coroutine(_statToModify, _modifier, _duration));
    }

    private IEnumerator StatModify_Coroutine(Stat _statToModify, float _modifier, float _duration)
    {
        _statToModify.AddModifier(_modifier);
        InventoryManager.instance.UpdateStatUI();

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
        InventoryManager.instance.UpdateStatUI();
    }

    public Stat GetStatByType(StatType _statType)
    {
        Stat stat = null;

        switch (_statType)
        {
            case StatType.strength: stat = strength; break;
            case StatType.agility: stat = agility; break;
            case StatType.intelligence: stat = intelligence; break;
            case StatType.vitality: stat = vitality; break;
            case StatType.damage: stat = damage; break;
            case StatType.critChance: stat = critChance; break;
            case StatType.critPower: stat = critPower; break;
            case StatType.maxHP: stat = maxHP; break;
            case StatType.armor: stat = armor; break;
            case StatType.evasion: stat = evasion; break;
            case StatType.magicResistance: stat = magicResistance; break;
            case StatType.fireDamage: stat = fireDamage; break;
            case StatType.iceDamage: stat = iceDamage; break;
            case StatType.lightningDamage: stat = lightningDamage; break;
            case StatType.cooldownDecreasePercentage: stat = cooldownDecreasePercentage; break;
        }
        return stat;
    }

    #region 获取最终属性值
    public float getMaxHP()
    {
        return maxHP.GetValue() + vitality.GetValue() * 5;
    }

    public float GetDamage()
    {
        return damage.GetValue() + strength.GetValue();
    }

    public float GetCritPower()
    {
        return critPower.GetValue() + strength.GetValue();
    }

    public float GetCritChance()
    {
        return critChance.GetValue() + agility.GetValue();
    }

    public float GetEvasion()
    {
        return evasion.GetValue() + agility.GetValue();
    }

    public float GetMagicResistance()
    {
        return magicResistance.GetValue() + intelligence.GetValue() * 3;
    }

    public float GetFireDamage()
    {
        return fireDamage.GetValue() + intelligence.GetValue();
    }

    public float GetIceDamage()
    {
        return iceDamage.GetValue() + intelligence.GetValue();
    }

    public float GetLightningDamage()
    {
        return lightningDamage.GetValue() + intelligence.GetValue();
    }
    #endregion
}
