using System.Collections.Generic;
using UnityEngine;

public class KeyBoardManager : MonoBehaviour, ISettingsSaveManager
{
    public static KeyBoardManager instance;

    public Dictionary<string, KeyCode> keyboardsDictionary;

    [SerializeField] private KeyboardList_UI keyboardList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        keyboardsDictionary = new Dictionary<string, KeyCode>();
    }

    private void Update()
    {

    }
    
    public void UpdateKeyboardListLanguage()
    {
        keyboardList.UpdateAllKeyboardOptionsLanguage();
    }
    public string UniformKeyboardName(string _behaveKeyboard_InUI)
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


    public void LoadData(SettingsData _data)
    {
        foreach (var search in _data.keyboardsDictionary)
        {
            keyboardsDictionary.Add(search.Key, search.Value);
        }
    }

    public void SaveData(ref SettingsData _data)
    {
        _data.keyboardsDictionary.Clear();

        foreach (var search in keyboardsDictionary)
        {
            _data.keyboardsDictionary.Add(search.Key, search.Value);
        }
    }
}
