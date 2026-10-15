using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "新套装", menuName = "数据/套装效果/黄金套装")]
public class GoldenSuitEffect : EquipmentSuitEffect
{
    public float currencyDropMultiple     = 1.2f;
    public float currencyDropMultipleHigh = 2f;

    public override void ExecuteSuitEffectWithTwo()
    {
        PlayerManager.instance.currencyDropMultiple *= currencyDropMultiple;
    }
    public override void ExecuteSuitEffectWithFour()
    {
        PlayerManager.instance.currencyDropMultiple *= currencyDropMultipleHigh;
    }
    public override void RemoveSuitEffectWithTwo()
    {
        PlayerManager.instance.currencyDropMultiple /= currencyDropMultiple;
    }
    public override void RemoveSuitEffectWithFour()
    {
        PlayerManager.instance.currencyDropMultiple /= currencyDropMultipleHigh;
    }
}
