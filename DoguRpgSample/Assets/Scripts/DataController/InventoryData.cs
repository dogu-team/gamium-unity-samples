using System;
using System.Collections.Generic;
using System.Linq;
using Data.Static;
using Unity.VisualScripting;
using Util;

namespace Data
{
    public class InventoryData : SingletonMonoBehavior<InventoryData>
    {
        private const string INVENTORY_DATA = "InventoryData";

        public List<ItemStack> items;
        public Action<ItemStack> onUnequip;
        public Action<ItemStack> onEquip;

        protected void Awake()
        {
            items = JsonPrefs.Load<List<ItemStack>>(INVENTORY_DATA);
            if (null == items)
            {
                items = new List<ItemStack>();
            }
        }

        public ItemStack GetData(ItemInfo itemInfo)
        {
            return items.FirstOrDefault(item => item.itemId == itemInfo.value.id);
        }


        public void IncrementItem(ItemInfo item, uint count)
        {
            IncrementItemInternal(item, count);
            Save();
        }

        public void DecrementItem(ItemInfo item, uint count)
        {
            DecrementItemInternal(item, count);
            Save();
        }


        public bool IsExchangable(ItemInfo sellItem, uint sellCount, ItemInfo buyItem, uint buyCount)
        {
            var itemStac = items.Find(i => i.itemId == sellItem.value.id);
            if (null == itemStac)
            {
                return false;
            }

            return itemStac.count >= sellCount;
        }


        public void ExchangeItem(ItemInfo sellItem, uint sellCount, ItemInfo buyItem, uint buyCount)
        {
            DecrementItemInternal(sellItem, sellCount);
            IncrementItemInternal(buyItem, buyCount);
            Save();
        }

        public ItemStack[] GetEquipments()
        {
            return items.Where(i =>
            {
                var item = i.GetItemInfo();
                if (null == item)
                {
                    return false;
                }

                if (item.type != ItemType.Equipment)
                {
                    return false;
                }

                return true;
            }).ToArray();
        }


        public ItemStack[] GetEquipedItems()
        {
            return items.Where(i =>
            {
                var item = i.GetItemInfo();
                if (null == item)
                {
                    return false;
                }

                if (item.type != ItemType.Equipment)
                {
                    return false;
                }

                if (!i.isEquiped)
                {
                    return false;
                }

                return true;
            }).ToArray();
        }

        public void Unequip(ItemStack itemStack)
        {
            itemStack.isEquiped = false;
            onUnequip?.Invoke(itemStack);
            Save();
        }


        public void Equip(ItemStack itemStack)
        {
            var itemInfo = itemStack.GetItemInfo();
            var equipItemInfo = itemInfo.value as EquipmentItemInfo;
            var equipVariadicInfo = equipItemInfo.GetVariadicInfo();

            var equipedItems = GetEquipedItems();
            foreach (var equipedItem in equipedItems)
            {
                var iterItemInfo = equipedItem.GetItemInfo();
                var iterEquipItemInfo = iterItemInfo.value as EquipmentItemInfo;
                var iterEquipVariadicInfo = iterEquipItemInfo.GetVariadicInfo();
                if (iterEquipVariadicInfo.equipmentPosition == equipVariadicInfo.equipmentPosition)
                {
                    Unequip(equipedItem);
                }
            }

            itemStack.isEquiped = true;
            onEquip?.Invoke(itemStack);
            Save();
        }

        private void IncrementItemInternal(ItemInfo item, uint count)
        {
            var itemStack = items.Find(i => i.itemId == item.value.id);
            if (null == itemStack)
            {
                itemStack = new ItemStack
                {
                    itemId = item.value.id,
                    count = 0,
                    isEquiped = false,
                };
                items.Add(itemStack);
            }

            itemStack.Increment(count);
        }

        private void DecrementItemInternal(ItemInfo item, uint count)
        {
            var itemStac = items.Find(i => i.itemId == item.value.id);
            if (null == itemStac)
            {
                throw new Exception($"Item not found: id:{item.value.id}");
            }

            itemStac.Decrement(count);
            if (itemStac.count == 0)
            {
                items.Remove(itemStac);
            }
        }


        private void Save()
        {
            JsonPrefs.Save(INVENTORY_DATA, items);
        }
    }
}