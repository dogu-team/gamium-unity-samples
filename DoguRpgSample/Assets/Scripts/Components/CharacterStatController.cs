using System;
using System.Linq;
using Data;
using Data.Static;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterStatController : MonoBehaviour
{
    public Stats stats;

    private void Awake()
    {
        stats = new Stats();
    }

    public void TakeDamage(long damage)
    {
        stats.Get(Stat.StatType.Health).Decrement(damage);
    }

    public Stat Get(Stat.StatType type)
    {
        return stats.Get(type);
    }

    public void ApplyCharacterInfo(Data.Static.CharacterInfo info, int level)
    {
        foreach (var statType in Enum.GetValues(typeof(Stat.StatType))
                     .Cast<Stat.StatType>())
        {
            var targetStat = stats.Get(statType);
            targetStat.RemoveModifier(Stat.ModifierType.CharacterInfo);

            var newModifierImpl = StatModifierRegistry.instance.Create(Stat.ModifierType.CharacterInfo);
            newModifierImpl.value = info.value.stats.Get(statType).GetPoint(level);
            targetStat.AddModifier(new StatModifier
            {
                type = Stat.ModifierType.CharacterInfo,
                value = newModifierImpl
            });
        }
    }

    public void ApplyUnequip(ItemStack itemStack)
    {
        var itemInfo = itemStack.GetItemInfo();
        var equipItemInfo = itemInfo.value as EquipmentItemInfo;
        var equipVariadicInfo = equipItemInfo.GetVariadicInfo();
        foreach (var stat in equipVariadicInfo.stats)
        {
            var targetStat = stats.Get(stat.type);
            foreach (var m in targetStat.Modifiers.ToList())
            {
                if (m.type != Stat.ModifierType.Equipment)
                {
                    continue;
                }

                var equipModifier = m.value as EquipmentStatModifier;
                if (equipModifier.itemInfo.value.id != itemInfo.value.id)
                {
                    continue;
                }

                targetStat.RemoveModifier(m);
            }
        }
    }

    public void ApplyEquip(ItemStack itemStack)
    {
        var itemInfo = itemStack.GetItemInfo();
        var equipItemInfo = itemInfo.value as EquipmentItemInfo;
        var equipVariadicInfo = equipItemInfo.GetVariadicInfo();
        foreach (var stat in equipVariadicInfo.stats)
        {
            var targetStat = stats.Get(stat.type);
            var newModifierImpl =
                StatModifierRegistry.instance.Create(Stat.ModifierType.Equipment) as EquipmentStatModifier;
            newModifierImpl.value = stat.value.GetPoint(equipItemInfo.upgradeLevel);
            newModifierImpl.itemInfo = itemInfo;
            targetStat.AddModifier(new StatModifier
            {
                type = Stat.ModifierType.Equipment,
                value = newModifierImpl
            });
        }
    }
}