using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

namespace UIToolkitDemo
{
    public class InventoryScreen : MenuScreen
    {
        public static event Action ScreenEnabled;
        public static event Action<EquipmentSO> GearSelected;
        public static event Action<Rarity, EquipmentType> GearFiltered;

        const string k_InventoryPanel = "inventory__panel";
        const string k_InventoryBackButton = "inventory__back-button";
        const string k_InventoryRarityDropdown = "inventory__rarity-dropdown";
        const string k_InventorySlotTypeDropdown = "inventory__slot-type-dropdown";

        // store gear items under scrollview, organized by row
        const string k_ScrollViewParent = "inventory__scrollview";

        const string k_GearItemClass = "gear-item";

        // gear items per row
        const int k_ItemsPerRow = 5;

        [Header("UI Asset")]
        [SerializeField] VisualTreeAsset m_GearItemAsset;

        ScrollView m_ScrollViewParent;

        // elements that represent each row
        List<VisualElement> m_RowParents;

        VisualElement m_InventoryBackButton;
        VisualElement m_InventoryPanel;

        DropdownField m_InventoryRarityDropdown;
        DropdownField m_InventorySlotTypeDropdown;

        // actively checked gear
        GearItemComponent m_SelectedGear;

        void OnEnable()
        {
            GearItemComponent.GearItemClicked += OnGearItemClicked;
            InventoryScreenController.InventorySetup += OnInventorySetup;
            InventoryScreenController.InventoryUpdated += OnInventoryUpdated;
        }

        void OnDisable()
        {
            GearItemComponent.GearItemClicked -= OnGearItemClicked;
            InventoryScreenController.InventorySetup -= OnInventorySetup;
            InventoryScreenController.InventoryUpdated -= OnInventoryUpdated;
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            m_InventoryBackButton = m_Root.Q(k_InventoryBackButton);
            m_InventoryPanel = m_Root.Q(k_InventoryPanel);
            m_InventoryRarityDropdown = m_Root.Q<DropdownField>(k_InventoryRarityDropdown);
            m_InventorySlotTypeDropdown = m_Root.Q<DropdownField>(k_InventorySlotTypeDropdown);

            // define row elements under the scrollview
            m_ScrollViewParent = m_Root.Q<ScrollView>(k_ScrollViewParent);
            m_RowParents = m_ScrollViewParent.Children() as List<VisualElement>;

            ClearRows();
        }

        protected override void RegisterButtonCallbacks()
        {
            m_InventoryBackButton?.RegisterCallback<ClickEvent>(CloseWindow);
            
            // register callbacks when value in a dropdown field changes
            m_InventoryRarityDropdown?.RegisterValueChangedCallback(UpdateFilters);
            m_InventorySlotTypeDropdown?.RegisterValueChangedCallback(UpdateFilters);
        }



        void ClearRows()
        {
            foreach (VisualElement elem in m_RowParents)
            {
                if (elem != null)
                {
                    elem.Clear();
                }
            }
        }

        // convert string to Rarity enum
        Rarity GetRarity(string rarityString)
        {

            Rarity rarity = Rarity.Common;

            if (!Enum.TryParse<Rarity>(rarityString, out rarity))
            {
                Debug.Log("String " + rarityString + " failed to convert");
            }
            return rarity;
        }

        // convert string to EquipmentType enum
        EquipmentType GetGearType(string gearTypeString)
        {

            EquipmentType gearType = EquipmentType.Weapon;

            if (!Enum.TryParse<EquipmentType>(gearTypeString, out gearType))
            {
                Debug.LogWarning("Converted " + gearTypeString + " failed to convert");
            }
            return gearType;
        }

        // hide/show gear items based on filters
        void UpdateFilters(ChangeEvent<string> evt)
        {
            EquipmentType gearType = GetGearType(m_InventorySlotTypeDropdown.value);
            Rarity rarity = GetRarity(m_InventoryRarityDropdown.value);
            GearFiltered?.Invoke(rarity, gearType);
        }

        // loop through the available slots and create a button for each gear item
        void ShowGearItems(List<EquipmentSO> gearToShow)
        {
            int maxCount = Mathf.Clamp(gearToShow.Count, 0, k_ItemsPerRow * m_RowParents.Count);

            ClearRows();

            // 
            for (int i = 0; i < maxCount; i++)
            {
                int parentRowIndex = i / k_ItemsPerRow;
                CreateGearItemButton(gearToShow[i], m_RowParents[parentRowIndex]);
            }

            HideEmptyRows();
        }

        // generate one item for the inventory and add a clickable button to select it
        void CreateGearItemButton(EquipmentSO gearData, VisualElement parentElement)
        {
            if (parentElement == null)
            {
                Debug.Log("InventoryScreen.CreateGearItemButton: missing parent element");
                return;
            }

            TemplateContainer gearUIElement = m_GearItemAsset.Instantiate();

            GearItemComponent gearItem = new GearItemComponent(gearData);

            // set visual element for gearItemComponent
            gearItem.SetVisualElements(gearUIElement);
            gearItem.SetGameData(gearUIElement);
            gearItem.RegisterButtonCallbacks();

            // add to the parent element
            parentElement.Add(gearUIElement);
        }

        // select or deselect an item
        void SelectGearItem(GearItemComponent gearItem, bool state)
        {
            if (gearItem == null)
                return;

            m_SelectedGear = (state) ? gearItem : null;
            gearItem.CheckItem(state);
        }

        // does a row contain at least one GearItem?
        bool IsRowEmpty(VisualElement rowToCheck)
        {
            VisualElement gearElement = rowToCheck.Q<VisualElement>(className: k_GearItemClass);
            return (gearElement == null);
        }

        // show rows that have gear, hide rows that don't
        void HideEmptyRows()
        {
            for (int i = 0; i < m_RowParents.Count; i++)
            {
                m_RowParents[i].style.display = IsRowEmpty(m_RowParents[i]) ? DisplayStyle.None : DisplayStyle.Flex;
            }
        }

        // methods to hide and show the screen
        public override void ShowScreen()
        {
            base.ShowScreen();
            ScreenEnabled?.Invoke();
            UpdateFilters(null);

            // add short transition
            m_InventoryPanel.transform.scale = new Vector3(0.1f, 0.1f, 0.1f);
            m_InventoryPanel.experimental.animation.Scale(1f, 200);
        }

        void CloseWindow(ClickEvent evt)
        {
            HideScreen();
        }

        public override void HideScreen()
        {
            base.HideScreen();

            AudioManager.PlayDefaultButtonSound();

            // set the selected Gear, notify the InventoryScreenController
            if (m_SelectedGear != null)
                GearSelected?.Invoke(m_SelectedGear.GearData);

            m_SelectedGear = null;

        }

        // event handling methods
        void OnInventorySetup()
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        void OnInventoryUpdated(List<EquipmentSO> gearToLoad)
        {
            ShowGearItems(gearToLoad);
        }

        void OnGearItemClicked(GearItemComponent gearItem)
        {

            AudioManager.PlayAltButtonSound();

            // deselect previously selected
            SelectGearItem(m_SelectedGear, false);

            // select the new gear item
            SelectGearItem(gearItem, true);
        }
    }
}