using System;
using Data;
using Data.Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemSlot : MonoBehaviour
    {
        public class ItemSlotParam
        {
            public Sprite placeHolderSprite;
            public bool isViewCount = true;
            public ItemStack itemStack;
            public Action onItemClick;
        }

        public Image placeholder;
        public Image icon;
        public Text upgradeLevelText;
        public Text countText;
        [NonSerialized] private ItemStack itemStack;
        [NonSerialized] private Action onItemClick;

        public void Refresh(ItemSlotParam param)
        {
            itemStack = param.itemStack;
            onItemClick = param.onItemClick;

            placeholder.gameObject.SetActive(false);
            if (null == param.itemStack)
            {
                if (null != param.placeHolderSprite)
                {
                    placeholder.sprite = param.placeHolderSprite;
                }

                placeholder.gameObject.SetActive(true);
                icon.gameObject.SetActive(false);
                upgradeLevelText.gameObject.SetActive(false);
                countText.gameObject.SetActive(false);
                return;
            }

            var itemInfo = ItemInfoList.Instance.GetEntry(itemStack.itemId);
            icon.sprite = itemInfo.value.icon;
            if (itemInfo.type == ItemType.Equipment)
            {
                var upgradeLevel = (itemInfo.value as EquipmentItemInfo).upgradeLevel;
                upgradeLevelText.text = $"Lv.{upgradeLevel.ToString()}";
                upgradeLevelText.gameObject.SetActive(true);
            }
            else
            {
                upgradeLevelText.gameObject.SetActive(false);
            }

            countText.text = itemStack.count.ToString();
            if (!param.isViewCount) countText.gameObject.SetActive(false);
            else countText.gameObject.SetActive(true);
        }

        public void OnClick()
        {
            if (null == itemStack)
            {
                return;
            }

            onItemClick?.Invoke();
        }
    }
}