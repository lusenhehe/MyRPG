using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillIconText_InGame_UI : MonoBehaviour
{
    [SerializeField] private string skillName;

    public void UpdateSkillIconText()
    {
        if (KeyBoardManager.instance.keyboardsDictionary.TryGetValue(skillName, out KeyCode keyboard))
        {
            GetComponent<TextMeshProUGUI>().text = UniformKeyboardNameForInGameUI(keyboard.ToString());

            //chinese needs to decrease font size a bit
            if (LanguageManager.instance.localeID == 1)
            {
                GetComponent<TextMeshProUGUI>().fontSize = 22;
            }
            //english
            else if (LanguageManager.instance.localeID == 0)
            {
                GetComponent<TextMeshProUGUI>().fontSize = 26;
            }
        }
    }


    private string UniformKeyboardNameForInGameUI(string _behaveKeyboard_InUI)
    {
        if (_behaveKeyboard_InUI.StartsWith("Alpha"))
        {
            _behaveKeyboard_InUI = _behaveKeyboard_InUI.Remove(0, 5);
        }

        //english
        if (LanguageManager.instance.localeID == 0)
        {
            if (_behaveKeyboard_InUI.Equals("Mouse0"))
            {
                _behaveKeyboard_InUI = "LMB";
            }

            if (_behaveKeyboard_InUI.Equals("Mouse1"))
            {
                _behaveKeyboard_InUI = "RMB";
            }

            if (_behaveKeyboard_InUI.StartsWith("Left"))
            {
                _behaveKeyboard_InUI = _behaveKeyboard_InUI.Remove(1, 3);
            }

            _behaveKeyboard_InUI = _behaveKeyboard_InUI.ToUpper();
        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            if (_behaveKeyboard_InUI.Equals("Mouse0"))
            {
                _behaveKeyboard_InUI = "左键";
            }

            if (_behaveKeyboard_InUI.Equals("Mouse1"))
            {
                _behaveKeyboard_InUI = "右键";
            }

            if (_behaveKeyboard_InUI.StartsWith("Left"))
            {
                _behaveKeyboard_InUI = _behaveKeyboard_InUI.Remove(0, 4);
                _behaveKeyboard_InUI = _behaveKeyboard_InUI.Insert(0, "  ");
            }
        }

        return _behaveKeyboard_InUI;
    }
}
