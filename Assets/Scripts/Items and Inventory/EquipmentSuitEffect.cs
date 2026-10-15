using UnityEngine;
public class EquipmentSuitEffect : ScriptableObject
{
    public string effectName;
    public Sprite sprite;
    [TextArea]
    public string descriptionWithTwo;
    [TextArea]
    public string description_ChineseWithTwo;
    [TextArea]
    public string descriptionWithFour;
    [TextArea]
    public string description_ChineseWithFour;

    public PlayerStats playerStats => PlayerManager.instance.player.GetComponent<PlayerStats>(); 
    public Player player => PlayerManager.instance.player; 
    public virtual void ExecuteEffect(int _amount){
        if(_amount >= 2 && _amount <= 3){
            ExecuteSuitEffectWithTwo();
        }
        else if(_amount >= 4){
            ExecuteSuitEffectWithFour();
        }
    }
    public virtual void RemoveEffect(int _amount){
        if(_amount >= 2 && _amount <= 3){
            RemoveSuitEffectWithTwo();
        }
        else if(_amount >= 4){
            RemoveSuitEffectWithFour();
        }
    }
    public virtual void ExecuteSuitEffectWithTwo()
    {
        
    }
    public virtual void ExecuteSuitEffectWithFour()
    {
        
    }
    public virtual void RemoveSuitEffectWithTwo()
    {
        
    }
    public virtual void RemoveSuitEffectWithFour()
    {
        
    }
}
