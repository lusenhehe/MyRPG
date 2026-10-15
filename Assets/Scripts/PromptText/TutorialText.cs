using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    private TextMeshPro tutorialText;

    [SerializeField] private List<string> boundBehaveNameList;
    [TextArea][SerializeField] private string originalText;
    [TextArea][SerializeField] private string originalText_Chinese;

    private void Awake()
    {
        tutorialText = GetComponent<TextMeshPro>();
    }


    private void Update()
    {
        UpdateTutorialText();
    }

    private void UpdateTutorialText()
    {
        string completedOriginalText = AddBehaveKeyboardNameToTutorialText(originalText);
        string completedOriginalText_Chinese = AddBehaveKeyboardNameToTutorialText(originalText_Chinese);

        //english
        if (LanguageManager.instance.localeID == 0)
        {
            tutorialText.text = completedOriginalText;
        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            tutorialText.text = completedOriginalText_Chinese;
        }
    }

    private string AddBehaveKeyboardNameToTutorialText(string _tutorialText)
    {
        for (int i = 0; i < boundBehaveNameList.Count; i++)
        {
            if (_tutorialText.Contains($"BehaveName{i}"))
            {
                string _keyboardName = KeyBoardManager.instance.keyboardsDictionary[boundBehaveNameList[i]].ToString();
                _keyboardName = KeyBoardManager.instance.UniformKeyboardName(_keyboardName);
                _tutorialText = _tutorialText.Replace($"BehaveName{i}", _keyboardName);
            }
        }

        return _tutorialText;
    }
}
