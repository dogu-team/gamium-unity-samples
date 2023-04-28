using System;
using System.Linq;
using UnityEngine;

namespace Data
{
    public static class SizeFixedEnumArray<TEnum, TValue>
        where TEnum : Enum
    {
        [Serializable]
        public class Entry
        {
            public TEnum type;
            public TValue value;
        }

        public static Entry[] Create()
        {
            var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
            var entries = new Entry[enumValues.Length];
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i] = new Entry
                {
                    type = enumValues[i],
                };
            }

            return entries;
        }


        public static void Validate(ref Entry[] entries)
        {
            var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
            Array.Resize(ref entries, enumValues.Length);
            for (int i = 0; i < enumValues.Length; i++)
            {
                if (null == entries[i])
                {
                    entries[i] = new Entry
                    {
                        type = enumValues[i],
                    };
                }

                if (!entries[i].type.Equals(enumValues[i]))
                {
                    entries[i].type = enumValues[i];
                }
            }

            for (int i = 0; i < entries.Length; i++)
            {
                if (null == entries[i].value)
                {
                    Debug.LogError(
                        $"EnumArray<{typeof(TEnum).Name}, {typeof(TValue).Name}>: Value at index {i} is null.");
                }
            }
        }

        public static void FillNull(ref Entry[] entries, Func<TValue> constructor)
        {
            for (int i = 0; i < entries.Length; i++)
            {
                if (null == entries[i].value)
                {
                    entries[i].value = constructor.Invoke();
                }
            }
        }
    }
}