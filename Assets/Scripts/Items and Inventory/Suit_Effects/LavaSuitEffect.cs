using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "熔岩迸发", menuName = "数据/套装效果/熔岩迸发")]
public class LavaSuitEffect : EquipmentSuitEffect
{
    public float FireDmgMultiple;
    public float FireDmgMultipleHigh;
    public override void ExecuteSuitEffectWithTwo()
    {
        playerStats.fireDamage.AddMultiplier(FireDmgMultiple);
    }
    public override void ExecuteSuitEffectWithFour()
    {
        playerStats.fireDamage.AddMultiplier(FireDmgMultipleHigh);
    }
    public override void RemoveSuitEffectWithTwo()
    {
        playerStats.fireDamage.RemoveMultiplier(FireDmgMultiple);
    }
    public override void RemoveSuitEffectWithFour()
    {
        playerStats.fireDamage.RemoveMultiplier(FireDmgMultipleHigh);
    }
}
