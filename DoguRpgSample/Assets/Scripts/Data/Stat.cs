using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

[Serializable]
public class Stat
{
    public enum StatType
    {
        Health,
        HealthRegen,
        Mana,
        ManaRegen,
        Stamina,
        Attack,
        AttackMagic,
        Defense,
        DefenseMagic,
        Dexiterity,
        Experience,
    }

    public enum ModifierType
    {
        CharacterInfo,
        Equipment
    }

    public Action<Stat> onValueChanged;
    public List<StatModifier> Modifiers => modifiers;
    [SerializeField] private long value;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    public Stat()
    {
        value = 0;
    }

    public long Value
    {
        get
        {
            long finalValue = value;
            modifiers.ForEach(x => finalValue += x.value.value);
            finalValue = Math.Clamp(finalValue, 0, long.MaxValue);
            return finalValue;
        }
    }

    public long ModifiersValue
    {
        get
        {
            long finalValue = 0;
            modifiers.ForEach(x => finalValue += x.value.value);
            finalValue = Math.Clamp(finalValue, 0, long.MaxValue);
            return finalValue;
        }
    }

    public void ResetValue()
    {
        value = 0;
        onValueChanged?.Invoke(this);
    }

    public void Increment(long amount)
    {
        value += amount;
        onValueChanged?.Invoke(this);
    }

    public void Decrement(long amount)
    {
        value -= amount;
        onValueChanged?.Invoke(this);
    }

    public void AddModifier(StatModifier modifier)
    {
        if (modifier.value == null || modifier.value.value == 0)
        {
            return;
        }

        modifiers.Add(modifier);
        onValueChanged?.Invoke(this);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        if (modifier.value == null || modifier.value.value == 0)
        {
            return;
        }

        modifiers.Remove(modifier);
        onValueChanged?.Invoke(this);
    }

    public void RemoveModifier(ModifierType type)
    {
        foreach (var modifier in modifiers.ToList())
        {
            if (modifier.type == type)
            {
                modifiers.Remove(modifier);
            }
        }

        onValueChanged?.Invoke(this);
    }
}