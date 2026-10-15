using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map_Button_UI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public UI ui => FindFirstObjectByType<UI>();
    private Map map;

    [SerializeField] private GameObject checkpointButton;
    private Transform checkpointListTransform;

    public void SetUp(Map _map, Transform _checkpointListTransform){
        map = _map;
        checkpointListTransform = _checkpointListTransform;
        GetComponentInChildren<TextMeshProUGUI>().text = _map.mapName;
        SetUpCheckpointHolder(ui, _map);
    }

    private void SetUpCheckpointHolder(UI ui, Map _map){

        for (int i = 0; i < checkpointListTransform.childCount; i++)
        {
            Destroy(checkpointListTransform.GetChild(i).gameObject);
        }
        Checkpoint[] checkpoints = _map.checkpoints;
        foreach (Checkpoint _checkpoint in checkpoints)
        {
            GameObject newCheckpoint = Instantiate(checkpointButton, checkpointListTransform);


            newCheckpoint.GetComponent<CheckPointButton_UI>().SetUP(ui, map, _checkpoint);
        }
    }

    
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        SetUpCheckpointHolder(ui, map);
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ui.clickAudioSource.GetComponent<AudioSource>().Play();
    }
}
