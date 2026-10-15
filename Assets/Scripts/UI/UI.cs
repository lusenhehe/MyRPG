using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour, ISettingsSaveManager
{
    public static UI instance;
    [Header("音频")]
    [SerializeField] public GameObject clickAudioSource;

    [Header("边框")]
    [SerializeField] public Sprite[] uiBorderSprites;

    [Header("UI")]
    [SerializeField] public GameObject character_UI;
    [SerializeField] private GameObject skillTree_UI;
    [SerializeField] private GameObject craft_UI;
    [SerializeField] private GameObject options_UI;
    [SerializeField] public GameObject ingame_UI;
    [SerializeField] public GameObject checkpoint_UI;

    [Header("Tooltips")]

    public SkillToolTip_UI skillToolTip;
    public ItemToolTip_UI itemToolTip;
    public StatToolTip_UI statToolTip;
    public CraftWindow_UI craftWindow;

    [Space]
    [Header("End Screen")]
    public FadeScreen_UI fadeScreen;
    [SerializeField] private GameObject tryAgain_UI;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject tryAgainButton;

    [Header("Thank you for playing")]
    [SerializeField] private GameObject thankYouForPlaying_UI;
    [SerializeField] private GameObject thankYouForPlayingText;
    [SerializeField] private TextMeshProUGUI achievedEndingText;
    [SerializeField] private GameObject returnToTitleButton;

    [Header("Audio Settings")]
    [SerializeField] private VolumeSlider_UI[] volumeSettings;

    [Header("Gameplay Settings")]
    [SerializeField] private GameplayOptionToggle_UI[] gameplayToggleSettings;

    private bool UIKeyFunctioning = true;

    private GameObject currentUI;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
            instance.gameObject.SetActive(true);
        }

        skillTree_UI.SetActive(true);
    }

    private void Start()
    {
        //No menu except ingame_UI is open in the beginning of game
        SwitchToMenu(ingame_UI);
        itemToolTip.itemToolTipWindow.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);

        fadeScreen.gameObject.SetActive(true);

        UIKeyFunctioning = true;
    }

    private void Update()
    {
        if(!GameManager.instance.consule_UI.consule_Input_UI.isInputting){
            if (UIKeyFunctioning && Input.GetKeyDown(/*KeyCode.C*/ KeyBoardManager.instance.keyboardsDictionary["Character"]))
            {
                OpenMenuByKeyBoard(character_UI);
            }

            //B for Craft UI
            if (UIKeyFunctioning && Input.GetKeyDown(/*KeyCode.B*/ KeyBoardManager.instance.keyboardsDictionary["Craft"]))
            {
                OpenMenuByKeyBoard(craft_UI);
            }

            //K for Skill Tree UI
            if (UIKeyFunctioning && Input.GetKeyDown(/*KeyCode.K*/ KeyBoardManager.instance.keyboardsDictionary["Skill"]))
            {
                OpenMenuByKeyBoard(skillTree_UI);
            }

            //If there's already a non-ingameUI open currently
            //Esc will close the UI and open ingame UI
            //else, Esc will open Options UI
            if (UIKeyFunctioning && Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentUI != ingame_UI)
                {
                    skillToolTip.gameObject.SetActive(false);
                    itemToolTip.itemToolTipWindow.SetActive(false);
                    statToolTip.gameObject.SetActive(false);
                    SwitchToMenu(ingame_UI);
                }
                else
                {
                    OpenMenuByKeyBoard(options_UI);
                }
            }
        }
        //C for Character UI
    }

    public void SwitchToMenu(GameObject _menu)
    {
        //close all the UIs
        for (int i = 0; i < transform.childCount; i++)
        {
            //keep black screen active
            bool isFadeScreen = (transform.GetChild(i).GetComponent<FadeScreen_UI>() != null);

            if (!isFadeScreen)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                currentUI = null;
            }
        }

        //set the target UI active
        if (_menu != null)
        {
            _menu.SetActive(true);
            currentUI = _menu;
            AudioManager.instance.PlaySFX(7, null);
        }

        if (_menu == ingame_UI)
            GameManager.instance?.PauseGame(false);
        else
            GameManager.instance?.PauseGame(true);
    }

    public void OpenMenuByKeyBoard(GameObject _menu)
    {
        //same as if(_menu != null && _menu.active == true)
        //activeSelf returns the active state of gameobject
        //Here means if re-entering (double press the same UI key) the same menu which is already open, then close the menu UI and open ingame UI
        if (_menu != null && _menu.activeSelf)
        {
            //_menu.SetActive(false);
            //currentUI = null;
            skillToolTip.gameObject.SetActive(false);
            itemToolTip.itemToolTipWindow.SetActive(false);
            statToolTip.gameObject.SetActive(false);
            SwitchToMenu(ingame_UI);
        }
        else if (_menu != null && !_menu.activeSelf)  //if the menu to switch is not open, then switch to that menu
        {
            SwitchToMenu(_menu);
        }
    }

    public Vector2 SetupToolTipPositionOffsetAccordingToMousePosition(float _xOffsetRate_left, float _xOffsetRate_right, float _yOffsetRate_up, float _yOffsetRate_down)
    {
        Vector2 mousePosition = Input.mousePosition;
        float _xOffset = 0;
        float _yOffset = 0;

        //if mouse is on the right side of the screen
        if (mousePosition.x >= Screen.width * 0.5)
        {
            _xOffset = -Screen.width * _xOffsetRate_left;
        }
        else //if mouse is on the left side of the screen
        {
            _xOffset = Screen.width * _xOffsetRate_right;
        }

        //if mouse is on the upper side of the screen
        if (mousePosition.y >= Screen.height * 0.5)
        {
            _yOffset = -Screen.height * _yOffsetRate_down;
        }
        else //if mouse is on the lower side of the screen
        {
            _yOffset = Screen.height * _yOffsetRate_up;
        }

        Vector2 toolTipPositionOffset = new Vector2(_xOffset, _yOffset);
        return toolTipPositionOffset;
    }

    public Vector2 SetupToolTipPositionOffsetAccordingToUISlotPosition(Transform _slotUITransform, float _xOffsetRate_left, float _xOffsetRate_right, float _yOffsetRate_up, float _yOffsetRate_down)
    {
        float _xOffset = 0;
        float _yOffset = 0;

        //if slotUI is on the right side of the screen
        if (_slotUITransform.position.x >= Screen.width * 0.5)
        {
            _xOffset = -Screen.width * _xOffsetRate_left;
        }
        else //if slotUI is on the left side of the screen
        {
            _xOffset = Screen.width * _xOffsetRate_right;
        }

        //if slotUI is on the upper side of the screen
        if (_slotUITransform.position.y >= Screen.height * 0.5)
        {
            _yOffset = -Screen.height * _yOffsetRate_down;
        }
        else //if slotUI is on the lower side of the screen
        {
            _yOffset = Screen.height * _yOffsetRate_up;
        }

        Vector2 toolTipPositionOffset = new Vector2(_xOffset, _yOffset);
        return toolTipPositionOffset;
    }

    public void SwitchToThankYouForPlaying(string _achievedEndingText)
    {
        UIKeyFunctioning = false;
        fadeScreen.FadeOut();
        StartCoroutine(ThankYouForPlayingCoroutine(_achievedEndingText));
    }

    private IEnumerator ThankYouForPlayingCoroutine(string _achievedEndingText)
    {
        yield return new WaitForSeconds(1.5f);
        thankYouForPlaying_UI.SetActive(true);

        thankYouForPlayingText.SetActive(true);
        achievedEndingText.text = _achievedEndingText;
        achievedEndingText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        returnToTitleButton.SetActive(true);

    }

    public void DeleteGameProgressionAndReturnToTitle()
    {
        StartCoroutine(DeleteGameProgressionAndReturnToTitle_Coroutine());
    }

    private IEnumerator DeleteGameProgressionAndReturnToTitle_Coroutine()
    {
        GameManager.instance.PauseGame(false);
        fadeScreen.FadeOut();
        SaveManager.instance.DeleteGameProgressionSavedData();

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("MainMenu");
    }

    public void SwitchToEndScreen()
    {
        //fadeScreen.gameObject.SetActive(true);
        UIKeyFunctioning = false;
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
    }

    private IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        tryAgain_UI.SetActive(true);
        endText.SetActive(true);
        yield return new WaitForSeconds(1f);
        tryAgainButton.SetActive(true);
    }

    public void RestartGame()
    {
        SaveManager.instance.SaveGame();
        GameManager.instance.RestartScene();
    }

    public void EnableUIKeyInput(bool _value)
    {
        UIKeyFunctioning = _value;
    }

    public void LoadData(SettingsData _data)
    {
        foreach (var search in _data.volumeSettingsDictionary)
        {
            foreach (var volume in volumeSettings)
            {
                if (volume.parameter == search.Key)
                {
                    volume.LoadVolumeSlider(search.Value);
                }
            }
        }
        foreach (var search in _data.gameplayToggleSettingsDictionary)
        {
            foreach (var toggle in gameplayToggleSettings)
            {
                if (toggle.optionName == search.Key)
                {
                    toggle.SetToggleValue(search.Value);
                }
            }
        }
    }

    public void SaveData(ref SettingsData _data)
    {
        //Audio setttings save
        _data.volumeSettingsDictionary.Clear();

        foreach (var volume in volumeSettings)
        {
            _data.volumeSettingsDictionary.Add(volume.parameter, volume.slider.value);
        }

        //gameplay toggle settings save
        _data.gameplayToggleSettingsDictionary.Clear();

        foreach (var toggle in gameplayToggleSettings)
        {
            _data.gameplayToggleSettingsDictionary.Add(toggle.optionName, toggle.GetToggleValue());
        }
    }
}
