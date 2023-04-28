using System;

namespace Data
{
    [Serializable]
    public class StatModifier : EnumbaseClassEntry<Stat.ModifierType, StatModifierBase>
    {
        public override StatModifierBase Create(Stat.ModifierType type)
        {
            return StatModifierRegistry.instance.Create(base.type);
        }
    }
}