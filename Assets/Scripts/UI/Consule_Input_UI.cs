using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Consule_Input_UI : MonoBehaviour
{
    public bool showConsole;
    public bool showHelp;
    public  bool isInputting;
    [SerializeField] public TMP_InputField InputField;
    [SerializeField] public GameObject consule_UI;

    public static DebugCommand giveCurrency;

    public static DebugCommand setMaxHealth;
    public static DebugCommand Help;
    public static DebugCommand ClearConsule;
    public static DebugCommand<string> SpawnEnemy;
    public static DebugCommand<string> AddConsule;
    public static DebugCommand<int> setCurrency;
    public List<object> commandList;

    public void OnToggleDebug(){
        consule_UI.SetActive(!consule_UI.activeSelf);
    }
    private void HandleInputText( string _input){
        GameManager.instance.consule_UI.AddDebug(_input);
    }
    private void HandleInputCommand(string _input){
        string[] properties = _input.Split(" ");
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if(_input.Contains(commandBase.commandId)){
                if(commandList[i] as DebugCommand != null){
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if(commandList[i] as DebugCommand<int> != null){
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
                else if(commandList[i] as DebugCommand<string> != null){
                    (commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                }
            }
        }
    }
    private void Awake(){
        giveCurrency = new DebugCommand("Give_Currency", "给玩家一些金钱", "get currency", () =>{
            PlayerManager.instance.currency += 5000;
        });
        ClearConsule = new DebugCommand("Clear_Consule", "清空控制台", "Clear your Consule", () =>{
            GameManager.instance.consule_UI.ClearConsule();
        });
        setCurrency = new DebugCommand<int>("Set_Currency", "给玩家一些金钱", "set currency<金钱数量>", (currency) =>{
            PlayerManager.instance.currency = currency;
        });
        SpawnEnemy = new DebugCommand<string>("Spawn_Enemy", "生成怪物", "spawn_Enemy<怪物名称>", (name) =>{
            GameObject enemy = EnemyManager.instance.getEnemyPrefab(name);
            Instantiate(enemy, PlayerManager.instance.player.transform.position, Quaternion.identity);
        });
        AddConsule = new DebugCommand<string>("Add_Consule", "输入信息", "add consule", (info) =>{
            GameManager.instance.consule_UI.AddDebug("<玩家>:"+info);
        });
        setMaxHealth = new DebugCommand("give_maxhealth", "给玩家一些金钱", "maxHealth", () =>{
            PlayerManager.instance.player.stats.currentHP = PlayerManager.instance.player.stats.getMaxHP();
        });
        Help = new DebugCommand("Help", "查看帮助", "Help", () =>{
            showHelp = true;
        });
        commandList = new List<object>
        {
            giveCurrency,
            setMaxHealth,
            setCurrency,
            Help,
            SpawnEnemy,
            AddConsule,
            ClearConsule,
        };
    }
    private void Update(){
        if(InputField.text != "" && !isInputting){
            isInputting = true;
        }
        if(InputField.text == ""){
            isInputting = false;
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            Debug.Log("enter");
            EndEdit(InputField.text);
        }
    }
    public void EndEdit(string input){
        if(input == ""){
            HandleInputText(input);
        }
        isInputting = false;
        InputField.text = string.Empty;
        if(input.Substring(0, 1) == "/"){
            input = input.Remove(0, 1);
            HandleInputCommand(input);
        }
        else{
            HandleInputText(input);
        }
    }
}
//Spawn_Enemy Slime_Big
