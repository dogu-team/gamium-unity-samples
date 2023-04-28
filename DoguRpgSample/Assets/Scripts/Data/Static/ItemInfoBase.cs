using System;
using UnityEngine;

namespace Data.Static
{
    [Serializable]
    public abstract class ItemInfoBase : EnumbaseClass<ItemType>
    {
        public string nickname;
        [TextArea] public string description;
        public Sprite icon;
        public uint price;
    }

    [Serializable]
    public class CoinItemInfo : ItemInfoBase
    {
        public override ItemType GetDataType() => ItemType.Coin;
    }

    [Serializable]
    public class PotionItemInfo : ItemInfoBase
    {
        public override ItemType GetDataType() => ItemType.Potion;
    }

    [Serializable]
    public class EquipmentItemInfo : ItemInfoBase
    {
        public override ItemType GetDataType() => ItemType.Equipment;
        [VariadicItemId] public int variadicItemId;
        public uint upgradeLevel;

        public EquipmentVariadicItemInfo GetVariadicInfo()
        {
            var itemInfo = VariadicItemInfoList.Instance.GetEntry(variadicItemId);
            return itemInfo.value as EquipmentVariadicItemInfo;
        }

        public uint GetUpgradePrice()
        {
            return price;
        }
    }


    public class ItemInfoRegistry : EnumbaseClassRegistry<ItemType, ItemInfoBase>
    {
        public static ItemInfoRegistry instance = new ItemInfoRegistry();

        public override EnumbaseClassRegistry<ItemType, ItemInfoBase> Instance()
        {
            return instance;
        }

        public override ItemInfoBase Create(ItemType type)
        {
            switch (type)
            {
                case ItemType.Coin:
                    return new CoinItemInfo();
                case ItemType.Potion:
                    return new PotionItemInfo();
                case ItemType.Equipment:
                    return new EquipmentItemInfo();
            }

            throw new Exception($"Invalid type: {type}");
        }
    }
}