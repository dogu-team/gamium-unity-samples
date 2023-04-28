using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Data
{
    [Serializable]
    public class Stats
    {
        public SizeFixedEnumArray<Stat.StatType, Stat>.Entry[] stats;

        public Stats()
        {
            stats = SizeFixedEnumArray<Stat.StatType, Stat>.Create();
            SizeFixedEnumArray<Stat.StatType, Stat>.FillNull(ref stats, () => new Stat());
        }

        public void Validate()
        {
            SizeFixedEnumArray<Stat.StatType, Stat>.Validate(ref stats);
        }

        public Stat Get(Stat.StatType type)
        {
            return stats[(int)type].value;
        }
    }
}