using System;
using Data;
using Data.Static;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryView : MonoBehaviour
    {
        public Text playerStatText;
        public EquipmentGridView equipmentGridView;
        public ItemGridView itemGridView;
        [NonSerialized] public Action onClosed;

        private void OnEnable()
        {
            Refresh();
        }

        private void Refresh()
        {
            var playerData = PlayerDataController.Instance.GetCurrentPlayerCharacter();
            var characterInfo = CharacterInfoList.Instance.GetEntry(playerData.characterId);
            var playerCharacterController = FindObjectOfType<PlayerCharacterController>();
            var stat = playerCharacterController.actionController.stat.stats;


            LevelInfoList.Instance.GetLevelInfoFromExp(playerData.Experience, out var levelInfo, out var nextLevelInfo);
            var coin = InventoryData.Instance.GetData(ItemInfoList.Instance.GetCoin());


            playerStatText.text = $" Name : {playerData.nickname}\n" +
                                  $" Class : {characterInfo.value.nickname}\n" +
                                  $" Level : {levelInfo.level}\n" +
                                  $" Exp : {playerData.Experience}\n" +
                                  $" Next Level Exp : {nextLevelInfo.miniumExperience}\n" +
                                  $" Health : {stat.Get(Stat.StatType.Health).Value}\n" +
                                  $" Attack : {stat.Get(Stat.StatType.Attack).Value}\n" +
                                  $" Defense : {stat.Get(Stat.StatType.Defense).Value}\n" +
                                  $" Coin: {coin.count}\n";
            var inventory = InventoryData.Instance;
            equipmentGridView.itemStacks = inventory.GetEquipedItems();
            equipmentGridView.onItemClick = OnEquipmentClick;
            equipmentGridView.Refresh();

            itemGridView.itemStacks = inventory.items.ToArray();
            itemGridView.onItemClick = OnItemClick;
            itemGridView.Refresh();
        }

        public void OnClose()
        {
            gameObject.SetActive(false);
            onClosed?.Invoke();
        }

        private void OnEquipmentClick(ItemStack itemStack)
        {
            var itemInfo = itemStack.GetItemInfo();
            var equipmentInfo = itemInfo.value as EquipmentItemInfo;

            var confirmPrefab = UIPrefabs.Instance.multipurposePopup;
            var go = Instantiate(confirmPrefab.gameObject, transform, false);
            var popup = go.GetComponent<MultipurposePopup>();
            var text = itemInfo.GetPrettyDescription();
            popup.Initialize(
                new MultipurposePopup.PopupParam
                {
                    icon = itemInfo.value.icon,
                    text = text,
                    positiveButtonText = "Unequip",
                    hasCounter = false,
                    buttonType = MultipurposePopup.PopupButtonType.OkCancel,
                    onCancel = null,
                    onConfirm = (count) => OnUnequip(itemStack)
                }
            );
        }


        private void OnItemClick(ItemStack itemStack)
        {
            var itemInfo = itemStack.GetItemInfo();
            switch (itemInfo.type)
            {
                case ItemType.Equipment:
                    ShowEquipmentItemPopup(itemStack);
                    return;
            }

            ShowNormalItemPopup(itemStack);
            return;
        }

        private void ShowNormalItemPopup(ItemStack itemStack)
        {
            var itemInfo = itemStack.GetItemInfo();
            var confirmPrefab = UIPrefabs.Instance.multipurposePopup;
            var go = Instantiate(confirmPrefab.gameObject, transform, false);
            var popup = go.GetComponent<MultipurposePopup>();
            var text = itemInfo.GetPrettyDescription();
            popup.Initialize(
                new MultipurposePopup.PopupParam
                {
                    icon = itemInfo.value.icon,
                    text = text,
                    hasCounter = false,
                    buttonType = MultipurposePopup.PopupButtonType.Close,
                    onCancel = null,
                    onConfirm = null
                }
            );
        }


        private void ShowEquipmentItemPopup(ItemStack itemStack)
        {
            var itemInfo = itemStack.GetItemInfo();
            var equipmentInfo = itemInfo.value as EquipmentItemInfo;

            var confirmPrefab = UIPrefabs.Instance.multipurposePopup;
            var go = Instantiate(confirmPrefab.gameObject, transform, false);
            var popup = go.GetComponent<MultipurposePopup>();
            var text = itemInfo.GetPrettyDescription();
            popup.Initialize(
                new MultipurposePopup.PopupParam
                {
                    icon = itemInfo.value.icon,
                    text = text,
                    positiveButtonText = "Equip",
                    hasCounter = false,
                    buttonType = MultipurposePopup.PopupButtonType.OkCancel,
                    onCancel = null,
                    onConfirm = (count) => OnEquip(itemStack)
                }
            );
        }

        private void OnUnequip(ItemStack itemStack)
        {
            InventoryData.Instance.Unequip(itemStack);
            Refresh();
        }


        private void OnEquip(ItemStack itemStack)
        {
            InventoryData.Instance.Equip(itemStack);
            Refresh();
        }
    }
}