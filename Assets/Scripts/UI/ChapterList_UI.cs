using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChapterList_UI : MonoBehaviour
{
    [SerializeField] List<Map> mapList;
    [SerializeField] private Transform mapListTransform;
    [SerializeField] private Transform checkpointListTransform;
    [SerializeField] private GameObject mapButtonPrefab;
    private void Start()
    {
        transform.parent.GetChild(0)?.GetComponent<ChapterList_UI>()?.SetupMapList();
    }

    public void SetupMapList()
    {
        for (int i = 0; i < mapListTransform.childCount; i++)
        {
            Destroy(mapListTransform.GetChild(i).gameObject);
        }
        for (int i = 0; i < mapList.Count; i++)
        {
            GameObject mapButton = Instantiate(mapButtonPrefab, mapListTransform);
            mapButton.GetComponent<Map_Button_UI>().SetUp(mapList[i], checkpointListTransform);
        }
    }
}
