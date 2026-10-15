using System.Collections.Generic;
using UnityEngine;

public class KeyboardConflictPromptWindowController : MonoBehaviour
{
    [SerializeField] private GameObject[] keyboardConflicts;

    //will consider at most 2 conflict keyboards
    public void SetupKeyboardConflictPromptWindow(KeyCode _keyCode)
    {
        List<string> _behaveNames = new List<string>();

        foreach (var search in KeyBoardManager.instance.keyboardsDictionary)
        {
            if (search.Value == _keyCode)
            {
                //behaveName here is always english
                _behaveNames.Add(search.Key);
            }
        }


        if (_behaveNames.Count >= 2)
        {
            //KeyboardConflictController[] controllers = GetComponentsInChildren<KeyboardConflictController>();

            //will consider at most 2 conflict keyboards
            int k = 0;

            for (int i = 0; i < keyboardConflicts.Length; i++)
            {
                keyboardConflicts[i].GetComponent<KeyboardConflictController>()?.SetupKeyboardConflict(_behaveNames[k], _keyCode.ToString());
                k++;

                if (k == 2)
                {
                    return;
                }
            }

        }
    }

    public void ClosePromptWindow()
    {
        Destroy(gameObject);
    }
}
