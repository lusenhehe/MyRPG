using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    [Header("Audio settings")]
    public SerializableDictionary<string, float> volumeSettingsDictionary;

    [Header("Keyboard Settings")]
    public SerializableDictionary<string, KeyCode> keyboardsDictionary;
    //public SerializableDictionary<string, KeyCode> keyboardsDictionary_Chinese; 

    [Header("Gameplay Settings")]
    public SerializableDictionary<string, bool> gameplayToggleSettingsDictionary;

    [Header("Language Settings")]
    public int localeID; //0 for english, 1 for chinese

    public SettingsData()
    {
        localeID = 1;

        //volumeSettingsDictionary<exposedParameter, value>
        volumeSettingsDictionary = new SerializableDictionary<string, float>();

        keyboardsDictionary = new SerializableDictionary<string, KeyCode>();
        SetupDefaultKeyboards();

        //gameplay toggle settings
        gameplayToggleSettingsDictionary = new SerializableDictionary<string, bool>();
    }

    private void SetupDefaultKeyboards()
    {
        keyboardsDictionary.Add("Attack", KeyCode.Mouse0);
        keyboardsDictionary.Add("Aim", KeyCode.Mouse1);
        keyboardsDictionary.Add("Flask", KeyCode.Alpha1);
        keyboardsDictionary.Add("Dash", KeyCode.LeftShift);
        keyboardsDictionary.Add("Parry", KeyCode.Q);
        keyboardsDictionary.Add("Crystal", KeyCode.F);
        keyboardsDictionary.Add("Blackhole", KeyCode.R);
        keyboardsDictionary.Add("Character", KeyCode.C);
        keyboardsDictionary.Add("Craft", KeyCode.B);
        keyboardsDictionary.Add("Skill", KeyCode.K);
    }
}
