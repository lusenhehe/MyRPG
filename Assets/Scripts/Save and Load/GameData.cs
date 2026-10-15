using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

[System.Serializable]
public class GameData
{
    public float currecny;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equippedEquipmentIDs;

    public SerializableDictionary<string, bool> checkpointsDictionary;
    public string closestActivatedCheckpointID;
    public string lastActivatedCheckpointID;
    public string lastActivatedMapName;

    [Header("Dropped Currency")]
    public float droppedCurrencyAmount;
    public Vector2 deathPosition;

    [Header("Map Elements")]
    public List<int> UsedMapElementIDList;

    public GameData()
    {
        this.currecny = 0;
        this.droppedCurrencyAmount = 0;
        this.deathPosition = Vector2.zero;

        UsedMapElementIDList = new List<int>();

        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();

        equippedEquipmentIDs = new List<string>();
        lastActivatedMapName = string.Empty;
        checkpointsDictionary = new SerializableDictionary<string, bool>();

        closestActivatedCheckpointID = string.Empty;
        lastActivatedCheckpointID = string.Empty;
    }
}
