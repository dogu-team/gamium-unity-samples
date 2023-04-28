using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    [RequireComponent(typeof(TabbedMenu))]
    public class ShopScreen : MenuScreen
    {
        // string IDs
        const string k_GoldScrollview = "shop__gold-scrollview";
        const string k_GemScrollview = "shop__gem-scrollview";
        const string k_PotionScrollview = "shop__potion-scrollview";
        const string k_ResourcePath = "GameData/GameIcons";

        [Header("Shop Item")]
        [Tooltip("ShopItem Element Asset to instantiate ")]
        [SerializeField] VisualTreeAsset m_ShopItemAsset;
        [SerializeField] GameIconsSO m_GameIconsData;

        // visual elements
        // each tab/parent element for each category of ShopItem
        VisualElement m_GoldScrollview;
        VisualElement m_GemsScrollview;
        VisualElement m_PotionScrollview;
        TabbedMenu m_TabbedMenu;

        void OnEnable()
        {
            ShopController.ShopTabRefilled += RefillShopTab;
        }

        void OnDisable()
        {
            ShopController.ShopTabRefilled -= RefillShopTab;
        }

        protected override void Awake()
        {
            base.Awake();

            // this ScriptableObject pairs data types (ShopItems, Skills, Rarity, Classes, etc.) with specific icons 
            // (default path = Resources/GameData/GameIcons)
            m_GameIconsData = Resources.Load<GameIconsSO>(k_ResourcePath);

            if (m_TabbedMenu == null)
                m_TabbedMenu = GetComponent<TabbedMenu>();
        }

        public override void ShowScreen()
        {
            base.ShowScreen();
            m_TabbedMenu?.SelectFirstTab();
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            m_GoldScrollview = m_Root.Q<VisualElement>(k_GoldScrollview);
            m_GemsScrollview = m_Root.Q<VisualElement>(k_GemScrollview);
            m_PotionScrollview = m_Root.Q<VisualElement>(k_PotionScrollview);
        }

        // fill a tab with content
        public void RefillShopTab(List<ShopItemSO> shopItems)
        {
            if (shopItems == null || shopItems.Count == 0)
                return;

            // generate items for each tab (gold, gems, potions)
            VisualElement parentTab = null;
            switch (shopItems[0].contentType)
            {
                case ShopItemType.Gold:
                    parentTab = m_GoldScrollview;
                    break;
                case ShopItemType.Gems:
                    parentTab = m_GemsScrollview;
                    break;
                default:
                    parentTab = m_PotionScrollview;
                    break;
            }

            parentTab.Clear();

            foreach (ShopItemSO shopItem in shopItems)
            {
                CreateShopItemElement(shopItem, parentTab);
            }
        }

        void CreateShopItemElement(ShopItemSO shopItemData, VisualElement parentElement)
        {
            if (parentElement == null || shopItemData == null || m_ShopItemAsset == null)
                return;

            // instantiate a new Visual Element from a template UXML
            TemplateContainer shopItemElem = m_ShopItemAsset.Instantiate();

            // sets up the VisualElements and game data per ShopItem
            ShopItemComponent shopItemController = new ShopItemComponent(m_GameIconsData, shopItemData);
            shopItemController.SetVisualElements(shopItemElem);
            shopItemController.SetGameData(shopItemElem);

            shopItemController.RegisterCallbacks();

            parentElement.Add(shopItemElem);

        }

        // TO-DO: replace with an Event Handler
        // jump to a specific tab (used from OptionsBar)
        public void SelectTab(string tabName)
        {
            m_TabbedMenu?.SelectTab(tabName);
        }
    }
}