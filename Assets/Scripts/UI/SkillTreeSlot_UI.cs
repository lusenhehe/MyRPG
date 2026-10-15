using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IGameProgressionSaveManager
{
    [SerializeField] private int skillPrice;
    [SerializeField] private List<string> boundBehaveNameList;

    [Header("English")]
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;

    [Header("Chinese")]
    [SerializeField] private string skillName_Chinese;
    [TextArea]
    [SerializeField] private string skillDescription_Chinese;

    [Space]
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked;

    [SerializeField] private SkillTreeSlot_UI[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlot_UI[] shouldBeLocked;

    private Image skillImage;
    private UI ui;

    private void OnValidate()
    {
        gameObject.name = $"技能树 - {skillName}";
    }

    private void Awake()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();
        //GetComponent<Button>().onClick.AddListener(() => UnlockSkill());
    }

    private void Start()
    {
        skillImage.color = lockedSkillColor;

        if (unlocked)
        {
            skillImage.color = Color.white;
        }
    }

    public void UnlockSkill()
    {
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                GameManager.instance.consule_UI.AddDebug("<系统>:前提技能未解锁");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                GameManager.instance.consule_UI.AddDebug("<系统>:相似技能已被解锁");
                return;
            }
        }

        //cannot re-unlock unlocked skill
        if (unlocked)
        {
            GameManager.instance.consule_UI.AddDebug("<系统>:你已经解锁了这个技能");
            return;
        }

        if (PlayerManager.instance.BuyIfAvailable(skillPrice) == false)
        {
            return;
        }

        unlocked = true;
        skillImage.color = Color.white;
        GameManager.instance.consule_UI.AddDebug($"<系统>:成功解锁 {skillName}");
    }
    private string AddBehaveKeyboardNameToDescription(string _skillDescription)
    {
        for (int i = 0; i < boundBehaveNameList.Count; i++)
        {
            if (_skillDescription.Contains($"BehaveName{i}"))
            {
                string _keyboardName = KeyBoardManager.instance.keyboardsDictionary[boundBehaveNameList[i]].ToString();
                _keyboardName = KeyBoardManager.instance.UniformKeyboardName(_keyboardName);
                _skillDescription = _skillDescription.Replace($"BehaveName{i}", _keyboardName);
            }
        }

        return _skillDescription;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        float defaultoffset = 0.2f;
        Vector2 offset = ui.SetupToolTipPositionOffsetAccordingToUISlotPosition(transform, defaultoffset, defaultoffset, defaultoffset, defaultoffset);

        ui.skillToolTip.transform.position = new Vector2(transform.position.x + offset.x, transform.position.y + offset.y);

        string completedSkillDescription = AddBehaveKeyboardNameToDescription(skillDescription);
        string completedSkillDescription_Chinese = AddBehaveKeyboardNameToDescription(skillDescription_Chinese);

        if (LanguageManager.instance.localeID == 0)
        {
            ui.skillToolTip.ShowToolTip(skillName, completedSkillDescription, skillPrice.ToString());
        }
        else if (LanguageManager.instance.localeID == 1)
        {
            ui.skillToolTip.ShowToolTip(skillName_Chinese, completedSkillDescription_Chinese, skillPrice.ToString());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UnlockSkill();
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }
}
