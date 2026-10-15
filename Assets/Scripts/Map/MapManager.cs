using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    [SerializeField] public List<GameObject> mapDir;
    [SerializeField] public Map localMap;
    [SerializeField] private Scene localScene => SceneManager.GetActiveScene();

    private void Awake()
    {
        GetLocalMap();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void GetLocalMap()
    {
        GameObject[] rootObjects = localScene.GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.GetComponent<Map>() != null)
                localMap = rootObject.GetComponent<Map>();
        }
    }

    private GameObject getMapbyMapName(string _mapName){
        GetLocalMap();
        foreach (var map in mapDir)
        {
            if(map.GetComponent<Map>()!= null){
                if(_mapName.Contains(map.GetComponent<Map>().mapName)){
                    return map;
                }
            }
        }
        return mapDir[0];
    }
    public void SwitchMap(string _mapName, bool _isTransformToclestCheckpoint = true, Checkpoint checkpoint = null)
    {
        CheckIfNoLocalMaps();
        GameManager.instance.consule_UI.AddDebug("<玩家>:从地图“" + localMap.mapName + "”传送至“" + _mapName+"”");
        if (_mapName.Contains(localMap.mapName) && _isTransformToclestCheckpoint){
            PlayerManager.instance.player.TransformToCloestActivatedCheckpoint(localMap);
            return;
        }
        else{
            GameObject localMapGameObject = getMapbyMapName(_mapName);
            if (localMap != null)
                localMap.DeleteSelf();
            if (localMapGameObject == null)
            {
                GameManager.instance.consule_UI.AddDebug("<系统>:找不到地图：" + _mapName);
                return;
            }
            localMap = localMapGameObject.GetComponent<Map>();
            Instantiate(localMapGameObject);

            if(checkpoint == null){
                PlayerManager.instance.player.transform.position = localMap.firstCheckpoint.transform.position;
            }
            else
            {
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
            }
            localMap.SetUp();
        }
    }

    private void CheckIfNoLocalMaps()
    {
        if (localMap == null)
        {
            Instantiate(mapDir[0], Vector3.zero, Quaternion.identity);
            localMap = mapDir[0].GetComponent<Map>();
        }
    }
}
