using System;
using Data;
using Data.Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProductSlot : MonoBehaviour
    {
        public Image icon;
        public Text upgradeLevelText;
        public Text productName;
        public Text productDescription;
        [NonSerialized] public ItemStack itemStack;
        [NonSerialized] public Action onProductClick;


        private void Start()
        {
            var itemInfo = itemStack.GetItemInfo();
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

            icon.sprite = itemInfo.value.icon;
            productName.text = itemInfo.value.nickname;
            productDescription.text = itemInfo.value.description;
        }

        public void OnClick()
        {
            onProductClick?.Invoke();
        }
    }
}