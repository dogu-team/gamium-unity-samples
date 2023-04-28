using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Static;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class UpgradeView : MonoBehaviour
    {
        public Image itemImage;
        public Text description;
        public StatChangeGrid statChangeGrid;
        public ItemGridView itemGridView;
        [NonSerialized] public Action onClosed;
        private ItemStack selectedItemStack;

        private void OnEnable()
        {
            Refresh();
        }

        private void Refresh()
        {
            var inventory = InventoryData.Instance;
            var coinItemData = inventory.GetData(ItemInfoList.Instance.GetCoin());
            var itemStacks = new List<ItemStack> { coinItemData };
            itemStacks.AddRange(inventory.GetEquipments());
            itemGridView.itemStacks = itemStacks.ToArray();
            itemGridView.onItemClick = OnItemClicked;
            itemGridView.Refresh();

            if (selectedItemStack == null)
            {
                itemImage.gameObject.SetActive(false);
                statChangeGrid.gameObject.SetActive(false);
                description.gameObject.SetActive(true);
                description.text = $"Select an item to upgrade";
                return;
            }

            var itemInfo = selectedItemStack.GetItemInfo();
            var equipItemInfo = itemInfo.value as EquipmentItemInfo;
            var equipVariadicItemInfo = equipItemInfo.GetVariadicInfo();

            itemImage.gameObject.SetActive(true);
            itemImage.sprite = itemInfo.value.icon;

            statChangeGrid.gameObject.SetActive(true);

            statChangeGrid.rows.Clear();
            statChangeGrid.rows.Add(new StatChangeGrid.Row
            {
                name = "Level",
                befValue = equipItemInfo.upgradeLevel.ToString(),
                afterValue = (equipItemInfo.upgradeLevel + 1).ToString()
            });
            foreach (var stat in equipVariadicItemInfo.stats)
            {
                statChangeGrid.rows.Add(new StatChangeGrid.Row
                {
                    name = stat.type.ToString(),
                    befValue = stat.value.GetPoint(equipItemInfo.upgradeLevel).ToString(),
                    afterValue = stat.value.GetPoint((equipItemInfo.upgradeLevel + 1)).ToString()
                });
            }

            statChangeGrid.Refresh();

            description.gameObject.SetActive(true);
            description.text = $"Equipment upgrades cost {equipItemInfo.GetUpgradePrice()} coins\n" +
                               $"{equipVariadicItemInfo.upgradeFailChances.GetPoint(equipItemInfo.upgradeLevel)}% chance of failure\n" +
                               $"are you sure?";
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
            onClosed?.Invoke();
        }

        public void OnUpgrade()
        {
            if (null == selectedItemStack)
            {
                return;
            }

            var confirmPrefab = UIPrefabs.Instance.multipurposePopup;
            var rand = Random.Range(0, 100);
            var itemInfo = selectedItemStack.GetItemInfo();
            var equipItemInfo = itemInfo.value as EquipmentItemInfo;
            var equipVariadicItemInfo = equipItemInfo.GetVariadicInfo();
            var failChance = equipVariadicItemInfo.upgradeFailChances.GetPoint(equipItemInfo.upgradeLevel);
            var coinItemData = InventoryData.Instance.GetData(ItemInfoList.Instance.GetCoin());
            if (coinItemData.count < equipItemInfo.GetUpgradePrice())
            {
                var go = Instantiate(confirmPrefab.gameObject, transform, false);
                var popup = go.GetComponent<MultipurposePopup>();
                popup.Initialize(
                    new MultipurposePopup.PopupParam
                    {
                        icon = itemInfo.value.icon,
                        text = "Not enough coins!",
                        hasCounter = false,
                        buttonType = MultipurposePopup.PopupButtonType.Close,
                        onCancel = null,
                        onConfirm = null
                    }
                );
                return;
            }

            if (rand < failChance)
            {
                InventoryData.Instance.DecrementItem(coinItemData.GetItemInfo(), equipItemInfo.GetUpgradePrice());
                var go = Instantiate(confirmPrefab.gameObject, transform, false);
                var popup = go.GetComponent<MultipurposePopup>();
                popup.Initialize(
                    new MultipurposePopup.PopupParam
                    {
                        icon = itemInfo.value.icon,
                        text = "Failed!",
                        hasCounter = false,
                        buttonType = MultipurposePopup.PopupButtonType.Close,
                        onCancel = null,
                        onConfirm = null
                    }
                );
                Refresh();
                return;
            }

            {
                var go = Instantiate(confirmPrefab.gameObject, transform, false);
                var popup = go.GetComponent<MultipurposePopup>();
                popup.Initialize(
                    new MultipurposePopup.PopupParam
                    {
                        icon = itemInfo.value.icon,
                        text = "Succeed!",
                        hasCounter = false,
                        buttonType = MultipurposePopup.PopupButtonType.Close,
                        onCancel = null,
                        onConfirm = null
                    }
                );
                var nextItem = ItemInfoList.Instance.entries.First(i =>
                {
                    if (i.type != ItemType.Equipment) return false;
                    var iterEquipInfo = i.value as EquipmentItemInfo;
                    if (iterEquipInfo.upgradeLevel != equipItemInfo.upgradeLevel + 1) return false;
                    var iterEquipVariadicItemInfo = iterEquipInfo.GetVariadicInfo();
                    if (iterEquipVariadicItemInfo.id != equipVariadicItemInfo.id) return false;
                    return true;
                });
                InventoryData.Instance.DecrementItem(selectedItemStack.GetItemInfo(), 1);
                InventoryData.Instance.DecrementItem(coinItemData.GetItemInfo(), equipItemInfo.GetUpgradePrice());
                InventoryData.Instance.IncrementItem(nextItem, 1);
                selectedItemStack = InventoryData.Instance.GetData(nextItem);
                Refresh();
            }
        }

        private void OnItemClicked(ItemStack itemStack)
        {
            var itemInfo = itemStack.GetItemInfo();
            var equipItemInfo = itemInfo.value as EquipmentItemInfo;
            if (null == equipItemInfo)
            {
                return;
            }

            var equipVariadicItemInfo = equipItemInfo.GetVariadicInfo();
            var nextLevel = equipItemInfo.upgradeLevel + 1;
            if (nextLevel >= equipVariadicItemInfo.upgradeMax)
            {
                ShowNotAvailablePopup(itemStack);
                return;
            }

            selectedItemStack = itemStack;
            Refresh();
        }

        private void ShowNotAvailablePopup(ItemStack itemStack)
        {
            var itemInfo = itemStack.GetItemInfo();
            var equipmentInfo = itemInfo.value as EquipmentItemInfo;
            var confirmPrefab = UIPrefabs.Instance.multipurposePopup;
            var go = Instantiate(confirmPrefab.gameObject, transform, false);
            var popup = go.GetComponent<MultipurposePopup>();
            popup.Initialize(
                new MultipurposePopup.PopupParam
                {
                    icon = itemInfo.value.icon,
                    text = "This item is already max level",
                    hasCounter = false,
                    buttonType = MultipurposePopup.PopupButtonType.Close,
                    onCancel = null,
                    onConfirm = null
                }
            );
        }
    }
}