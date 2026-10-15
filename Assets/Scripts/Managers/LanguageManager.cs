using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour, ISettingsSaveManager
{
    public static LanguageManager instance;
    [SerializeField] public int localeID;

    public Dictionary<string, string> EnglishToChineseKeyboardsDictionary;
    public Dictionary<string, string> EnglishToChineseEquipmentTypeDictionary;
    public Dictionary<string, string> EnglishToChineseStatDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            localeID = 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        EnglishToChineseKeyboardsDictionary = new Dictionary<string, string>();
        EnglishToChineseEquipmentTypeDictionary = new Dictionary<string, string>();
        EnglishToChineseStatDictionary = new Dictionary<string, string>();

        SetupEnglishToChineseKeyboardsDictionary();
        SetupEnglishToChineseEquipmentTypeDictionary();
        SetupEnglishToChineseStatDictionary();
    }

    public void SetTextLanguageByLocaleID(int _localeID)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
    }

    private void SetupEnglishToChineseKeyboardsDictionary()
    {
        EnglishToChineseKeyboardsDictionary.Add("Attack", "攻击");
        EnglishToChineseKeyboardsDictionary.Add("Aim", "瞄准");
        EnglishToChineseKeyboardsDictionary.Add("Flask", "面罩");
        EnglishToChineseKeyboardsDictionary.Add("Dash", "冲刺");
        EnglishToChineseKeyboardsDictionary.Add("Parry", "格挡");
        EnglishToChineseKeyboardsDictionary.Add("Crystal", "水晶");
        EnglishToChineseKeyboardsDictionary.Add("Blackhole", "黑洞");
        EnglishToChineseKeyboardsDictionary.Add("Character", "玩家");
        EnglishToChineseKeyboardsDictionary.Add("Craft", "合成");
        EnglishToChineseKeyboardsDictionary.Add("Skill", "技能");
    }

    private void SetupEnglishToChineseEquipmentTypeDictionary()
    {
        EnglishToChineseEquipmentTypeDictionary.Add("Weapon", "武器");
        EnglishToChineseEquipmentTypeDictionary.Add("Chest", "盔甲");
        EnglishToChineseEquipmentTypeDictionary.Add("Ring", "戒指");
        EnglishToChineseEquipmentTypeDictionary.Add("Flask", "面罩");
        EnglishToChineseEquipmentTypeDictionary.Add("Neck", "项链");
        EnglishToChineseEquipmentTypeDictionary.Add("Feet", "靴子");
        EnglishToChineseEquipmentTypeDictionary.Add("Shield", "盾牌");
        EnglishToChineseEquipmentTypeDictionary.Add("Helmet", "头盔");
    }

    private void SetupEnglishToChineseStatDictionary()
    {
        EnglishToChineseStatDictionary.Add("Strength", "力量");
        EnglishToChineseStatDictionary.Add("Agility", "活力");
        EnglishToChineseStatDictionary.Add("Intelligence", "智力");
        EnglishToChineseStatDictionary.Add("Vitality", "活力");
        EnglishToChineseStatDictionary.Add("Damage", "伤害");
        EnglishToChineseStatDictionary.Add("Crit Chance", "暴击率");
        EnglishToChineseStatDictionary.Add("Crit Power", "暴击伤害");
        EnglishToChineseStatDictionary.Add("Max HP", "最大生命值");
        EnglishToChineseStatDictionary.Add("Evasion", "闪避");
        EnglishToChineseStatDictionary.Add("Armor", "盔甲");
        EnglishToChineseStatDictionary.Add("Magic Resist", "魔法抵抗");
        EnglishToChineseStatDictionary.Add("Fire Dmg", "火焰伤害");
        EnglishToChineseStatDictionary.Add("Ice Dmg", "冰霜伤害");
        EnglishToChineseStatDictionary.Add("Lightning Dmg", "闪电伤害");
    }

    public string GetEnglishNameByChinese(string _chineseName)
    {
        string result = string.Empty;

        foreach (var search in EnglishToChineseKeyboardsDictionary)
        {
            if (search.Value == _chineseName)
            {
                result = search.Key;
            }
        }

        return result;
    }

    public string TranslateItemStatInfoFromEnglishToChinese(string _itemStatInfo)
    {
        foreach (var search in EnglishToChineseStatDictionary)
        {
            string stat_English = search.Key;

            _itemStatInfo = _itemStatInfo.Replace(stat_English, EnglishToChineseStatDictionary[stat_English]);
        }

        return _itemStatInfo;
    }

    public void LoadData(SettingsData _data)
    {
        localeID = _data.localeID;

        SetTextLanguageByLocaleID(localeID);
    }

    public void SaveData(ref SettingsData _data)
    {
        _data.localeID = localeID;
    }
}
