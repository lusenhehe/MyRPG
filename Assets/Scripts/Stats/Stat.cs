using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;

    public List<float> multipliers = new List<float>() { 1 };

    public List<float> modifiers;

    public float GetValue()
    {
        float finalValue = baseValue;
        finalValue += modifiers.Sum();
        float multiplicativeResult = multipliers.Aggregate(1.0f, (acc, val) => acc * val);
        return Mathf.Max(0, finalValue * multiplicativeResult); // 确保返回值非负
    }

    public void SetDefaultValue(float _defaultValue)
    {
        baseValue = _defaultValue;
    }

    public void AddModifier(float _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(float _modifier)
    {
        modifiers.Remove(_modifier);
    }

    public void AddMultiplier(float _multiplier)
    {
        multipliers.Add(_multiplier);
    }

    public void RemoveMultiplier(float _multiplier)
    {
        multipliers.Remove(_multiplier);
    }
}
