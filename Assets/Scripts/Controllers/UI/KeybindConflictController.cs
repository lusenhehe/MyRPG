using TMPro;
using UnityEngine;

public class KeyboardConflictController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI behaveName_InUI;
    [SerializeField] protected TextMeshProUGUI behaveKeyboard_InUI;

    private string behaveName;
    private string behaveKeyboard;

    public void SetupKeyboardConflict(string _behaveName, string _behaveKeyboard)
    {
        behaveName = _behaveName;
        behaveKeyboard = _behaveKeyboard;

        behaveName_InUI.text = _behaveName;
        behaveKeyboard_InUI.text = _behaveKeyboard;

        TranslateBehaveNameAndUniformBehaveKeyboardName();

        //behaveKeyboard_InUI.text = UniformKeyboardName(_behaveKeyboard);
    }

    public void TranslateBehaveNameAndUniformBehaveKeyboardName()
    {
        //chinese
        if (LanguageManager.instance.localeID == 1)
        {
            behaveName_InUI.text = LanguageManager.instance.EnglishToChineseKeyboardsDictionary[behaveName];
        }
        //english
        else if (LanguageManager.instance.localeID == 0)
        {
            behaveName_InUI.text = behaveName;
        }

        //uniform keyboard will auto detect language and translate
        behaveKeyboard_InUI.text = UniformKeyboardName(behaveKeyboard);
    }

    private string UniformKeyboardName(string _behaveKeyboard_InUI)
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
                _behaveKeyboard_InUI = "Mouse Left";
            }

            if (_behaveKeyboard_InUI.Equals("Mouse1"))
            {
                _behaveKeyboard_InUI = "Mouse Right";
            }

            if (_behaveKeyboard_InUI.StartsWith("Left"))
            {
                _behaveKeyboard_InUI = _behaveKeyboard_InUI.Insert(4, " ");
            }
        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            if (_behaveKeyboard_InUI.Equals("Mouse0"))
            {
                _behaveKeyboard_InUI = "鼠标左键";
            }

            if (_behaveKeyboard_InUI.Equals("Mouse1"))
            {
                _behaveKeyboard_InUI = "鼠标右键";
            }

            if (_behaveKeyboard_InUI.StartsWith("Left"))
            {
                _behaveKeyboard_InUI = _behaveKeyboard_InUI.Remove(0, 4);
                _behaveKeyboard_InUI = _behaveKeyboard_InUI.Insert(0, "L");
            }
        }

        return _behaveKeyboard_InUI;
    }

}
