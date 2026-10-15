using System.Text;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ItemType
{
    Material,
    Equipment
}
public enum ItemQuality
{
    Normal,
    Common,
    Special,
    Golden,
    Legendary,
    UR,
}
[CreateAssetMenu(fileName = "新物品", menuName = "数据/物品")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public ItemQuality itemQuality;
    public string itemName;
    public string itemName_Chinese;
    public Sprite icon;
    public string itemID;
    // public int level = 1; // 当前等级
    // public int maxLevel; // 最高等级，根据物品品质计算
    // public int[] upgradeCost; // 每级升级所需的货币数量
    // public float[] upgradeSuccessRate; // 每级升级的成功率
    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();
    private void OnValidate()
    {
#if UNITY_EDITOR  //meaning the code inside this # (macro) will only be executed in unity editor, and won't be compiled when building the game
        string path = AssetDatabase.GetAssetPath(this);
        // maxLevel = (int)GetItemQualityNum(itemQuality) * 5;
        itemID = AssetDatabase.AssetPathToGUID(path);  //GUID means global unique identifier for the asset
#endif
    }

    public virtual string GetItemStatInfoAndEffectDescription()
    {
        return "";
    }
    public string GetItemQualityName(ItemQuality _quality){
        switch (_quality)
        {
            case ItemQuality.Normal:
                return "普通";
            case ItemQuality.Common:
                return "平凡";
            case ItemQuality.Special:
                return "特殊";
            case ItemQuality.Golden:
                return "完美";
            case ItemQuality.Legendary:
                return "传奇";
            case ItemQuality.UR:
                return "天仙";
            default:
                return "未知";
        }
    }
    public int GetItemQualityNum(ItemQuality _quality){
        switch (_quality)
        {
            case ItemQuality.Normal:
                return 0;
            case ItemQuality.Common:
                return 1;
            case ItemQuality.Special:
                return 2;
            case ItemQuality.Golden:
                return 3;
            case ItemQuality.Legendary:
                return 4;
            case ItemQuality.UR:
                return 5;
            default:
                return 0;
        }
    }
}
