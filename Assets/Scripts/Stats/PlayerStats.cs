using UnityEngine;

public class PlayerStats : CharacterStats
{
    public string playerName;
    private Player player;
    private PlayerFX playerFX;

    private float playerDefaultMoveSpeed;

    protected override void Awake()
    {
        base.Awake();
        playerFX = GetComponent<PlayerFX>();
    }
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
        playerDefaultMoveSpeed = player.moveSpeed;
    }
    protected override void Update()
    {
        base.Update();
        
    }

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);

        if (_targetStats.GetComponent<Enemy>() != null)
        {
            Player player = PlayerManager.instance.player;
            int playerComboCounter = player.primaryAttackState.comboCounter;
            if (player.stateMachine.currentState == player.primaryAttackState)
            {
                PlayerAttackFx(playerComboCounter);
            }
        }
    }

    private void PlayerAttackFx(int playerComboCounter)
    {
        switch (playerComboCounter)
        {
            case 0:
                playerFX.ScreenShake(playerFX.lightShakeDuration, playerFX.lightShakeStrength);
                playerFX.AttackPause(5);
                break;
            case 1:
                playerFX.ScreenShake(playerFX.mediunumShakeDuration, playerFX.mediunumShakeStrength);
                playerFX.AttackPause(5);
                break;
            case 2:
                playerFX.ScreenShake(playerFX.heavyShakeDuration, playerFX.heavyShakeStrength);
                playerFX.AttackPause(5);
                break;
            default:
                break;
        }
    }


    public override void TakeDamage(float _damage, Transform _attacker, Transform _attackee, AttackType _attackType)
    {
        //if (isInvincible)
        //{
        //    return;
        //}

        float takenDamage = DecreaseHPBy(_damage, _attackType);
        _attackee.GetComponent<Entity>()?.DamageFlashEffect();

        SlowerPlayerMoveSpeedForTime(0.2f);
        if (takenDamage >= player.stats.getMaxHP() * 0.3f)
        {
            _attackee.GetComponent<Entity>()?.DamageKnockbackEffect(_attacker, _attackee);
            player.fx.ScreenShake(player.fx.heavyShakeDuration, player.fx.heavyShakeStrength);
        }

        if (currentHP <= 0 && !isDead)
        {
            Die();
        }
    }

    private void SlowerPlayerMoveSpeedForTime(float _duration)
    {
        player.moveSpeed = player.moveSpeed * _duration;

        Invoke("ReturnToDefaultMoveSpeed", _duration);
    }

    private void ReturnToDefaultMoveSpeed()
    {
        player.moveSpeed = playerDefaultMoveSpeed;
    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        GameManager.instance.droppedCurrencyAmount = PlayerManager.instance.GetCurrentCurrency();
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenrateDrop();
    }

    public override float DecreaseHPBy(float _takenDamage, AttackType attackType)
    {
        base.DecreaseHPBy(_takenDamage, attackType);

        int randomIndex = Random.Range(34, 36);
        AudioManager.instance.PlaySFX(randomIndex, player.transform);

        ItemData_Equipment currentArmor = InventoryManager.instance.GetEquippedEquipmentByType(EquipmentType.Chest);

        if (currentArmor != null)
        {
            //currentArmor.ExecuteItemEffect(player.transform);
            InventoryManager.instance.UseArmorEffect_ConsiderCooldown(player.transform);
        }

        return _takenDamage;
    }

    public override void OnEvasion()
    {
        GameManager.instance.consule_UI.AddDebug("<玩家>:玩家闪避了伤害");
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _cloneAttackDamageMultipler, Transform _cloneTransform)
    {
        if (TargetCanEvadeThisAttack(_targetStats))
        {
            return;
        }

        float _totalDamage = damage.GetValue() + strength.GetValue();

        AttackType crit = CanCrit();

        if (crit == AttackType.CritDmg)
        {
            GameManager.instance.consule_UI.AddDebug("<玩家>:暴击伤害");
            _totalDamage = CalculatCritDamage(_totalDamage);
        }

        fx.CreateHitFX(_targetStats.transform, crit);

        //clone attack damage should be less than player's damage
        if (_cloneAttackDamageMultipler > 0)
        {
            _totalDamage = Mathf.RoundToInt(_totalDamage * _cloneAttackDamageMultipler);
        }
        _totalDamage = CheckTargetArmor(_targetStats, _totalDamage);
        _totalDamage = CheckTargetVulnerability(_targetStats, _totalDamage);

        _targetStats.TakeDamage(_totalDamage, _cloneTransform, _targetStats.transform, crit);
    }
}
