using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public GameObject[] enemies;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject getEnemyPrefab(string _name){
        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i].name.Contains(_name)){
                return enemies[i];
            }
        }
        return enemies[0];
    }
}
