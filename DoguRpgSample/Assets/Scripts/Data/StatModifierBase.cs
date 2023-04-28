using System;
using Data.Static;

namespace Data
{
    [Serializable]
    public abstract class StatModifierBase : EnumbaseClass<Stat.ModifierType>
    {
        public long value;
    }

    [Serializable]
    public class CharacterInfoStatModifier : StatModifierBase
    {
        public override Stat.ModifierType GetDataType() => Stat.ModifierType.CharacterInfo;
    }


    [Serializable]
    public class EquipmentStatModifier : StatModifierBase
    {
        public override Stat.ModifierType GetDataType() => Stat.ModifierType.Equipment;
        public ItemInfo itemInfo;
    }


    public class StatModifierRegistry : EnumbaseClassRegistry<Stat.ModifierType, StatModifierBase>
    {
        public static StatModifierRegistry instance = new StatModifierRegistry();

        public override EnumbaseClassRegistry<Stat.ModifierType, StatModifierBase> Instance()
        {
            return instance;
        }

        public override StatModifierBase Create(Stat.ModifierType type)
        {
            switch (type)
            {
                case Stat.ModifierType.CharacterInfo:
                    return new CharacterInfoStatModifier();
                case Stat.ModifierType.Equipment:
                    return new EquipmentStatModifier();
            }

            throw new Exception($"Invalid type: {type}");
        }
    }
}