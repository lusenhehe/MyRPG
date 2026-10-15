using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "回复效果", menuName = "数据/物品效果/回复效果")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healHP = Mathf.RoundToInt(playerStats.getMaxHP() * healPercent);
        playerStats.IncreaseHPBy(healHP);
    }

    //public override void ExecuteEffect_NoHitNeeded()
    //{
    //    PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

    //    int healHP = Mathf.RoundToInt(playerStats.getMaxHP() * healPercent);
    //    playerStats.IncreaseHPBy(healHP);
    //}
}
