using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameProgressionSaveManager
{
    public static PlayerManager instance;
    public Player player;

    public float currency;
    
    public float currencyDropMultiple = 1f;

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

    private void Update()
    {

        if (currency >= 999999)
        {
            currency = 999999;
        }

    }

    public bool BuyIfAvailable(int _price)
    {
        if (currency < _price)
        {
            GameManager.instance.consule_UI.AddDebug("<系统>:金币不够了");
            return false;
        }

        currency -= _price;
        return true;
    }

    public float GetCurrentCurrency()
    {
        return currency;
    }

    public void LoadData(GameData _data)
    {
        this.currency = _data.currecny;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currecny = this.currency;
    }
}
