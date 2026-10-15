using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CheckPointButton_UI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    private UI ui;
    private Map map;
    private Checkpoint checkpoint;

    public void SetUP(UI _ui, Map _map, Checkpoint _checkpoint){
        ui = _ui;
        map = _map;
        checkpoint = _checkpoint;
        
        GetComponentInChildren<TextMeshProUGUI>().text = _checkpoint.name;
    }

    public virtual void OnPointerDown(PointerEventData eventData){
        Transform_UI transform_UI = FindFirstObjectByType<Transform_UI>();
        transform_UI.SetUp(ui, map, checkpoint);
    }
    public virtual void OnPointerEnter(PointerEventData eventData){
        ui.clickAudioSource.GetComponent<AudioSource>().Play();
    }
}
