using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "新套装", menuName = "数据/套装效果/锁链套装")]
public class ChainSuitEffect : EquipmentSuitEffect
{
    public float damageBuff = 10;
    public float damageBuffHigh = 30;
    public override void ExecuteSuitEffectWithTwo()
    {
        playerStats.damage.AddModifier(damageBuff);
    }
    public override void ExecuteSuitEffectWithFour()
    {
        playerStats.damage.AddModifier(damageBuffHigh);
    }
    public override void RemoveSuitEffectWithTwo()
    {
        playerStats.damage.RemoveModifier(damageBuff);
    }
    public override void RemoveSuitEffectWithFour()
    {
        playerStats.damage.RemoveModifier(damageBuffHigh);
    }
}
