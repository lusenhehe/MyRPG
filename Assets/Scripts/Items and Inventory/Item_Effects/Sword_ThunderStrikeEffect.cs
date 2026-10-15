using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "剑气-雷电", menuName = "数据/物品效果/剑气-雷电")]
public class Sword_ThunderStrikeEffect : ItemEffect
{

    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyTransform.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
