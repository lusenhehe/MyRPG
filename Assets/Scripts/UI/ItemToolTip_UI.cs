using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip_UI : MonoBehaviour
{
    [SerializeField] public GameObject itemToolTipWindow;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemQualityText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemStatInfo;
    [SerializeField] private TextMeshProUGUI itemSuitEffectInfo;
    [SerializeField] private Image itemQualityImage;

    [Range(1, 72)]
    [SerializeField] private int originalItemNameFontSize;

    private void Start()
    {
        gameObject.SetActive(true);
        itemNameText.fontSize = originalItemNameFontSize;
    }

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null){
            return;
        }

        itemTypeText.text = item.equipmentType.ToString();
        itemStatInfo.text = item.GetItemStatInfoAndEffectDescription();
        int index = item.GetItemQualityNum(item.itemQuality);
        itemQualityImage.sprite = FindFirstObjectByType<UI>().uiBorderSprites[index];
        itemQualityImage.color = new Color(255,255,255,1);
        FillinItemEffect(item);

        if (LanguageManager.instance.localeID == 0)
        {
            itemNameText.text = item.itemName;
        }
        else if (LanguageManager.instance.localeID == 1)
        {
            itemNameText.text = item.itemName_Chinese;
            FillinItemQuality(item);
            itemTypeText.text = LanguageManager.instance.EnglishToChineseEquipmentTypeDictionary[itemTypeText.text];
            itemStatInfo.text = LanguageManager.instance.TranslateItemStatInfoFromEnglishToChinese(itemStatInfo.text);
        }


        if (itemNameText.text.Length > 12)
        {
            itemNameText.fontSize *= 0.8f;
        }
        else
        {
            itemNameText.fontSize = originalItemNameFontSize;
        }

        itemToolTipWindow.SetActive(true);
    }

    private void FillinItemQuality(ItemData_Equipment item)
    {
        if (LanguageManager.instance.localeID == 0)
        {
            itemQualityText.text = "Quality：" + item.itemQuality;
        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            itemQualityText.text = "品质：" + item.GetItemQualityName(item.itemQuality);
            
        }
    }
    private void FillinItemEffect(ItemData_Equipment item)
    {
        if (item.equipmentSuitEffect == null)
        {
            itemSuitEffectInfo.text = "";
            return;
        }
        if (LanguageManager.instance.localeID == 0)
        {
            itemSuitEffectInfo.text = $"Suit Effect:{item.equipmentSuitEffect.effectName}\n[Two Piece]{item.equipmentSuitEffect.descriptionWithTwo}\n[Four Piece]{item.equipmentSuitEffect.descriptionWithFour}";
        }
        else if (LanguageManager.instance.localeID == 1)
        {
            itemSuitEffectInfo.text = $"套装效果:{item.equipmentSuitEffect.effectName}\n[两件套]{item.equipmentSuitEffect.description_ChineseWithTwo}\n[四件套]{item.equipmentSuitEffect.description_ChineseWithFour}";            
        }
    }

    public void HideToolTip()
    {
        itemNameText.fontSize = originalItemNameFontSize;
        itemToolTipWindow.SetActive(false);
    }
}
