using TMPro;
using UnityEngine;

public class Consule_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    public Consule_Input_UI consule_Input_UI;
    public void AddDebug(string _addString){
        text.text += "\n"+_addString;
    }
    public void ClearConsule(){
        text.text = "";
    }
}
