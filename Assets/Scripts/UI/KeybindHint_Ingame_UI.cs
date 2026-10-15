using TMPro;
using UnityEngine;

public class KeyboardHint_Ingame_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyboardText;
    [SerializeField] private string keyboardTextPreset;
    [SerializeField] private string behaveName;


    private void Start()
    {
        GetKeyboardIfAvailable();
    }

    private void OnEnable()
    {
        GetKeyboardIfAvailable();
    }

    private void GetKeyboardIfAvailable()
    {
        if (KeyBoardManager.instance == null)
        {
            return;
        }

        if (KeyBoardManager.instance.keyboardsDictionary.ContainsKey(behaveName))
        {
            keyboardText.text = UniformKeyboardName(KeyBoardManager.instance.keyboardsDictionary[behaveName].ToString());
            keyboardText.text = keyboardTextPreset + keyboardText.text;
        }
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
