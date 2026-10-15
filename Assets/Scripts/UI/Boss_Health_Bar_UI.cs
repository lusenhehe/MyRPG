using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss_HealthBar_UI : MonoBehaviour
{
    private RectTransform barTransform;
    public TextMeshProUGUI bossName;
    private Slider slider;
    public bool isOn;
    public CharacterStats myStats;

    private void Awake()
    {
        barTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        if(isOn)
            myStats.onHealthChanged += UpdateHPUI;
    }

    private void OnDisable()
    {
        if(isOn)
        {
            myStats.onHealthChanged -= UpdateHPUI;
        }
    }

    private void Start()
    {
        UpdateHPUI();
        UpdateBossName();
    }
    private void UpdateBossName(){
        bossName.text = myStats.gameObject.name;
    }
    private void UpdateHPUI()
    {
        slider.maxValue = myStats.getMaxHP();
        slider.value = myStats.currentHP;
    }
    private void FlipUI()
    {
        barTransform.Rotate(0, 180, 0);
    }
}
