using System;
using Data.Static;

namespace Data
{
    [Serializable]
    public class ItemStack
    {
        [ItemId] public int itemId;
        public uint count;
        public bool isEquiped;

        public void Increment(uint argCount)
        {
            ulong sum = Math.Clamp((ulong)count + (ulong)argCount, uint.MinValue, uint.MaxValue);
            count = (uint)sum;
        }

        public void Decrement(uint argCount)
        {
            if (count < argCount)
            {
                count = 0;
                return;
            }

            count -= argCount;
        }

        public ItemInfo GetItemInfo()
        {
            return ItemInfoList.Instance.GetEntry(itemId);
        }
    }
}