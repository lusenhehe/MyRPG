using UnityEngine;
using TMPro;

public class Transform_UI : MonoBehaviour
{
    private UI ui;
    private Map map;

    private Checkpoint checkpoint;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TransformButton_UI transformButton;
    public void SetUp(UI _ui, Map _map, Checkpoint checkpoint){
        ui = _ui;
        map = _map;
        text.text = checkpoint.name;
        transformButton.SetUpTransformButton(ui, _map, checkpoint);
    }
}
