using System;
using Data;
using Data.Static;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UI
{
    public class ShopView : MonoBehaviour
    {
        public LayoutGroup productsLayout;
        public ScrollRect scrollRect;
        public ItemGridView itemGridView;

        [NonSerialized] public ItemStack[] productStacks = new ItemStack[] { };
        [NonSerialized] public Action onClosed;

        private void OnEnable()
        {
            foreach (Transform child in productsLayout.transform)
            {
                Destroy(child.gameObject);
            }

            var productSlotPrefab = UIPrefabs.Instance.productSlotPrefab;
            foreach (var product in productStacks)
            {
                var go = Instantiate(productSlotPrefab.gameObject, productsLayout.transform, false);
                var slot = go.GetComponent<ProductSlot>();
                slot.onProductClick = () => ShowBuyConfirm(slot);
                slot.itemStack = product;
            }

            scrollRect.verticalNormalizedPosition = 1.0f;

            var inventory = InventoryData.Instance;
            itemGridView.itemStacks = inventory.items.ToArray();
            itemGridView.onItemClick = (item) => ShowSellConfirm(item);
            itemGridView.Refresh();
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
            onClosed?.Invoke();
        }

        private void ShowBuyConfirm(ProductSlot slot)
        {
            var itemInfo = slot.itemStack.GetItemInfo();
            var confirmPrefab = UIPrefabs.Instance.multipurposePopup;
            var go = Instantiate(confirmPrefab.gameObject, transform, false);
            var popup = go.GetComponent<MultipurposePopup>();
            var text = itemInfo.GetPrettyDescription();
            text += $"\nBuy {itemInfo.value.nickname} for {itemInfo.value.price} coins?";
            popup.Initialize(
                new MultipurposePopup.PopupParam
                {
                    icon = itemInfo.value.icon,
                    text = text,
                    hasCounter = false,
                    buttonType = MultipurposePopup.PopupButtonType.OkCancel,
                    onCancel = null,
                    onConfirm = (count) => Buy(slot)
                }
            );
        }

        private void Buy(ProductSlot slot)
        {
            var itemInfo = slot.itemStack.GetItemInfo();
            var inventory = InventoryData.Instance;
            var coin = ItemInfoList.Instance.GetCoin();
            if (inventory.IsExchangable(coin, itemInfo.value.price, itemInfo, 1))
            {
                inventory.ExchangeItem(coin, itemInfo.value.price, itemInfo, 1);
                itemGridView.itemStacks = inventory.items.ToArray();
                itemGridView.Refresh();
            }
        }

        private void ShowSellConfirm(ItemStack itemStack)
        {
            var confirmPrefab = UIPrefabs.Instance.multipurposePopup;
            var go = Instantiate(confirmPrefab.gameObject, transform, false);
            var popup = go.GetComponent<MultipurposePopup>();
            var itemInfo = ItemInfoList.Instance.GetEntry(itemStack.itemId);
            var text = itemInfo.GetPrettyDescription();
            var sellPrice = itemInfo.value.price / 2;

            text += $"\nSell {itemInfo.value.nickname} for {sellPrice} coins?";
            popup.Initialize(
                new MultipurposePopup.PopupParam
                {
                    icon = itemInfo.value.icon,
                    text = text,
                    hasCounter = false,
                    buttonType = MultipurposePopup.PopupButtonType.OkCancel,
                    onCancel = null,
                    onConfirm = (count) => Sell(itemStack)
                }
            );
        }

        private void Sell(ItemStack itemStack)
        {
            var inventory = InventoryData.Instance;
            var coin = ItemInfoList.Instance.GetCoin();
            var itemInfo = ItemInfoList.Instance.GetEntry(itemStack.itemId);
            var sellPrice = itemInfo.value.price / 2;

            if (inventory.IsExchangable(itemInfo, 1, coin, sellPrice))
            {
                inventory.ExchangeItem(itemInfo, 1, coin, sellPrice);
                itemGridView.itemStacks = inventory.items.ToArray();
                itemGridView.Refresh();
            }
        }
    }
}