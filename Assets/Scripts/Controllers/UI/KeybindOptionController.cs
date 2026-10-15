using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardOptionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI behaveName_InUI;
    [SerializeField] protected TextMeshProUGUI behaveKeyboard_InUI;
    [SerializeField] private Button keyboardButton;
    [Space]
    [SerializeField] private GameObject keyboardConflictPromptWindowPrefab;

    //all these 2 are english
    private string behaveName;
    private string behaveKeyboard;

    private void Start()
    {
        keyboardButton.onClick.AddListener(ChangeKeyboard);
    }

    //_behaveName is english, cuz KeyboardManager.instance.keyboardsDictionary only supports english
    public void SetupKeyboardOption(string _behaveName, string _behaveKeyboard)
    {
        behaveName = _behaveName;
        behaveKeyboard = _behaveKeyboard;

        behaveName_InUI.text = _behaveName;
        behaveKeyboard_InUI.text = _behaveKeyboard;

        TranslateBehaveNameAndUniformBehaveKeyboardName();
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

        //UniformKeyboaedName will auto detect language and translate
        behaveKeyboard_InUI.text = UniformKeyboaedName(behaveKeyboard);
    }

    private string UniformKeyboaedName(string _behaveKeyboard_InUI)
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

    public void ChangeKeyboard()
    {
        StartCoroutine(ChangeKeyboardInput());
    }

    private IEnumerator ChangeKeyboardInput()
    {
        //show awaiting input prompt window UI
        if (LanguageManager.instance.localeID == 0)
        {
            behaveKeyboard_InUI.text = "Awaiting input";
        }
        else if (LanguageManager.instance.localeID == 1)
        {
            behaveKeyboard_InUI.text = "等待输入";
        }

        UI.instance?.EnableUIKeyInput(false);
        MainMenu_UI.instance?.EnableKeyInput(false);

        yield return new WaitUntil(CheckInput);

        keyboardButton.onClick.RemoveAllListeners();

        //unity has a bug, have to do null check like this here
        if (SkillPanel_InGame_UI.instance != null)
        {
            SkillPanel_InGame_UI.instance?.UpdateAllSkillIconTexts();
        }

        yield return new WaitWhile(HasAnyKey);

        keyboardButton.interactable = true;
        keyboardButton.onClick.AddListener(ChangeKeyboard);

        UI.instance?.EnableUIKeyInput(true);
        MainMenu_UI.instance?.EnableKeyInput(true);
    }

    private bool CheckInput()
    {
        keyboardButton.interactable = false;

        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
            {
                if (keycode == KeyCode.Escape)
                {
                    behaveKeyboard_InUI.text = UniformKeyboaedName(KeyBoardManager.instance.keyboardsDictionary[behaveName].ToString());
                    GameManager.instance.consule_UI.AddDebug("<系统>:取消键位变换");
                    return true;
                }

                behaveKeyboard_InUI.text = UniformKeyboaedName(keycode.ToString());
                behaveKeyboard = keycode.ToString();
                GameManager.instance.consule_UI.AddDebug($"<系统>:{behaveName_InUI.text} 变为了 {keycode.ToString()}");

                KeyBoardManager.instance.keyboardsDictionary[behaveName] = keycode;

                if (HasKeyboardConflict(keycode))
                {
                    GameObject newKeyboardConflictPromptWindow = Instantiate(keyboardConflictPromptWindowPrefab, transform.parent.parent.parent.parent);
                    newKeyboardConflictPromptWindow.GetComponent<KeyboardConflictPromptWindowController>()?.SetupKeyboardConflictPromptWindow(keycode);
                }

                return true;
            }

        }
        return false;
    }


    private bool HasAnyKey()
    {
        if (Input.anyKey || Input.anyKeyDown)
        {
            return true;
        }

        return false;
    }

    private bool HasKeyboardConflict(KeyCode _keycode)
    {
        foreach (var search in KeyBoardManager.instance.keyboardsDictionary)
        {
            //will not conflict with itself
            if (search.Key == behaveName)
            {
                continue;
            }

            if (search.Value == _keycode)
            {
                return true;
            }
        }

        return false;
    }

}
