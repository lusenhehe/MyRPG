using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransformButton_UI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    private UI ui;
    private Map map;
    private Checkpoint checkpoint;
    public void SetUpTransformButton(UI _ui, Map _map, Checkpoint _checkpoint){
        map = _map;
        ui = _ui;
        checkpoint = _checkpoint;
        GetComponentInChildren<TextMeshProUGUI>().text = "传送";
    }
    public virtual void OnPointerDown(PointerEventData eventData){

        if(map == null)
            return;
        MapManager.instance.SwitchMap(map.mapName, false, checkpoint);
        ui.SwitchToMenu(ui.ingame_UI);
    }
    public virtual void OnPointerEnter(PointerEventData eventData){
        if(ui == null)
            ui = FindFirstObjectByType<UI>();
        ui.clickAudioSource.GetComponent<AudioSource>().Play();
    }
}
