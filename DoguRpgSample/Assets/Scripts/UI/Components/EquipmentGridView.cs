using System;
using System.Linq;
using Data;
using Data.Static;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class EquipmentGridView : MonoBehaviour
    {
        public LayoutGroup itemsLayout;
        [NonSerialized] public ItemStack[] itemStacks;
        [NonSerialized] public Action<ItemStack> onItemClick;


        public void Refresh()
        {
            TransformUtil.DestroyChildren(itemsLayout.transform);

            var equipmentEntries = SizeFixedEnumArray<EquipmentPosition, ItemStack>.Create();
            foreach (var itemStack in itemStacks)
            {
                var item = itemStack.GetItemInfo();
                if (null == item)
                {
                    continue;
                }

                if (item.type != ItemType.Equipment)
                {
                    continue;
                }

                var equipmentItemInfo = item.value as EquipmentItemInfo;
                var equipmentItemMetaInfo = equipmentItemInfo.GetVariadicInfo();
                equipmentEntries[(int)equipmentItemMetaInfo.equipmentPosition].value = itemStack;
            }


            var itemSlotPrefab = UIPrefabs.Instance.itemSlotPrefab;
            foreach (var entry in equipmentEntries)
            {
                var go = Instantiate(itemSlotPrefab.gameObject, itemsLayout.transform, false);
                var slot = go.GetComponent<ItemSlot>();
                var placeHolder = UISprites.Instance.equipmentSprites[(int)entry.type].value;

                slot.Refresh(new ItemSlot.ItemSlotParam
                {
                    placeHolderSprite = placeHolder,
                    itemStack = entry.value,
                    isViewCount = false,
                    onItemClick = () => onItemClick?.Invoke(entry.value)
                });
            }
        }
    }
}