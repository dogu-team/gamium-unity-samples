using System;
using UnityEngine;

namespace Data.Static
{
    [Serializable]
    public class LootInfo
    {
        public ItemInfo item;
        public uint amount;
        [Range(0.0f, 1.0f)] public float chance;
    }
}