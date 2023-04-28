using System;

namespace Data.Static
{
    [Serializable]
    public class StaticStats
    {
        public SizeFixedEnumArray<Stat.StatType, ChartValue>.Entry[] stats;

        public StaticStats()
        {
            stats = SizeFixedEnumArray<Stat.StatType, ChartValue>.Create();
            SizeFixedEnumArray<Stat.StatType, ChartValue>.FillNull(ref stats, () => new ChartValue());
        }

        public void Validate()
        {
            SizeFixedEnumArray<Stat.StatType, ChartValue>.Validate(ref stats);

            foreach (var stat in stats)
            {
                ChartValue.Validate(stat.value);
            }
        }

        public ChartValue Get(Stat.StatType type)
        {
            return stats[(int)type].value;
        }
    }
}