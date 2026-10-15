using UnityEngine;

public class GameFinishCollider : MonoBehaviour
{
    [SerializeField] private Map nextMap;
    [SerializeField] private string achievedEndingText_Chinese;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log(nextMap.GetMapName());
            MapManager.instance.SwitchMap(nextMap.GetMapName());
        }
    }
}
