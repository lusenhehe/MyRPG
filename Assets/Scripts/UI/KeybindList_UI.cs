using System.Collections.Generic;
using UnityEngine;

public class KeyboardList_UI : MonoBehaviour
{
    [SerializeField] private GameObject keyboardOptionPrefab;

    public List<GameObject> keyboardOptionList { get; private set; } = new List<GameObject>();

    private void Start()
    {
        GenerateKeyboardList();
    }

    private void GenerateKeyboardList()
    {
        foreach (var search in KeyBoardManager.instance.keyboardsDictionary)
        {
            if (search.Key != null)
            {
                GameObject keyboardOption = Instantiate(keyboardOptionPrefab, transform);
                keyboardOption.GetComponent<KeyboardOptionController>().SetupKeyboardOption(search.Key, search.Value.ToString());

                keyboardOptionList.Add(keyboardOption);
            }
        }
    }

    public void UpdateAllKeyboardOptionsLanguage()
    {
        if (keyboardOptionList.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < keyboardOptionList.Count; i++)
        {
            keyboardOptionList[i].GetComponent<KeyboardOptionController>()?.TranslateBehaveNameAndUniformBehaveKeyboardName();
        }
    }
}
