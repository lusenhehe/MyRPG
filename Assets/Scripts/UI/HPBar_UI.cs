using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar_UI : MonoBehaviour
{
    public Image imageEffect;

    public Image imageHP;

    public float buffTime = 0.4f;
    private Entity entity;
    private RectTransform barTransform;

    private CharacterStats myStats;
    private TextMeshProUGUI text;

    private void Awake()
    {
        barTransform = GetComponent<RectTransform>();
        entity  = GetComponentInParent<Entity>();
        myStats = GetComponentInParent<CharacterStats>();
        text    = GetComponentInChildren<TextMeshProUGUI>();
        UpdateHealthBar();
    }

    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        if (entity != null)
        {
            entity.onFlipped -= FlipUI;
        }

        if (myStats != null)
        {
            myStats.onHealthChanged -= UpdateHealthBar;
        }
    }

    private void Start()
    {
        UpdateHealthBar();
    }
    private Coroutine updateCoroutine;

    private void UpdateHealthBar()
    {
        if (myStats.currentHP < 0)
            myStats.currentHP = 0;
        imageHP.fillAmount = myStats.currentHP / myStats.getMaxHP();
        text.text = $"{myStats.currentHP}/{myStats.getMaxHP()}";
        if(updateCoroutine != null){
            StopCoroutine(updateCoroutine);
        }

        updateCoroutine = StartCoroutine(UpdateHpEffect());
    }
    private IEnumerator UpdateHpEffect()
    {
        float effectLength = imageEffect.fillAmount - imageHP.fillAmount;
        float elapsedTime = 0;

        while (elapsedTime < buffTime && effectLength != 0)
        {
            elapsedTime += Time.deltaTime;
            imageEffect.fillAmount = Mathf.Lerp(imageHP.fillAmount + effectLength, imageHP.fillAmount, elapsedTime / buffTime);
            yield return null;
        }

        imageEffect.fillAmount = imageHP.fillAmount;
    }

    private void FlipUI()
    {
        barTransform.Rotate(0, 180, 0);
    }
}
