using System;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class ItemGridView : MonoBehaviour
    {
        public LayoutGroup itemsLayout;
        [NonSerialized] public ItemStack[] itemStacks;
        [NonSerialized] public Action<ItemStack> onItemClick;


        public void Refresh()
        {
            TransformUtil.DestroyChildren(itemsLayout.transform);

            var itemSlotPrefab = UIPrefabs.Instance.itemSlotPrefab;
            foreach (var itemStack in itemStacks)
            {
                var go = Instantiate(itemSlotPrefab.gameObject, itemsLayout.transform, false);
                var slot = go.GetComponent<ItemSlot>();
                slot.Refresh(new ItemSlot.ItemSlotParam
                {
                    itemStack = itemStack,
                    onItemClick = () => onItemClick?.Invoke(itemStack)
                });
            }
        }
    }
}