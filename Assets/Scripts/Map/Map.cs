using System;
using UnityEngine;
[Serializable]
public class Map : MonoBehaviour
{
    [SerializeField] public string mapName;

    [SerializeField] public string nextMapName;

    [SerializeField] private Transform checkpointTransform;
    [SerializeField] public Transform firstCheckpoint;
    [SerializeField] public Checkpoint[] checkpoints;
    public string GetMapName() => mapName;
    private void OnValidate(){
        checkpoints = checkpointTransform.GetComponentsInChildren<Checkpoint>();
    }
    public void SetUp(){
    }
    public void DeleteSelf(){
        Destroy(gameObject);
    }
}
