using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace UIToolkitDemo
{
    // presenter/controller for the ShopScreen
    public class ShopController : MonoBehaviour
    {
        // notify ShopScreen (pass ShopItem data + screen pos of buy button)
        public static event Action<ShopItemSO, Vector2> ShopItemPurchasing;
        public static event Action<List<ShopItemSO>> ShopTabRefilled;

        [Tooltip("Path within the Resources folders for MailMessage ScriptableObjects.")]
        [SerializeField] string m_ResourcePath = "GameData/ShopItems";

        // ScriptableObject game data from Resources
        List<ShopItemSO> m_ShopItems = new List<ShopItemSO>();

        // game data filtered in categories
        List<ShopItemSO> m_GoldShopItems = new List<ShopItemSO>();
        List<ShopItemSO> m_GemShopItems = new List<ShopItemSO>();
        List<ShopItemSO> m_PotionShopItems = new List<ShopItemSO>();

        void OnEnable()
        {
            ShopItemComponent.ShopItemClicked += OnTryBuyItem;
        }

        void OnDisable()
        {
            ShopItemComponent.ShopItemClicked -= OnTryBuyItem;
        }

        void Start()
        {
            LoadShopData();
            UpdateView();
        }

        // fill the ShopScreen with data
        void LoadShopData()
        {
            // load the ScriptableObjects from the Resources directory (default = Resources/GameData/MailMessages)
            m_ShopItems.AddRange(Resources.LoadAll<ShopItemSO>(m_ResourcePath));

            // sort them by type
            m_GoldShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.Gold).ToList();
            m_GemShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.Gems).ToList();
            m_PotionShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.HealthPotion || c.contentType == ShopItemType.LevelUpPotion).ToList();

            m_GoldShopItems = SortShopItems(m_GoldShopItems);
            m_GemShopItems = SortShopItems(m_GemShopItems);
            m_PotionShopItems = SortShopItems(m_PotionShopItems);
        }

        List<ShopItemSO> SortShopItems(List<ShopItemSO> originalList)
        {
            return originalList.OrderBy(x => x.cost).ToList();
        }

        void UpdateView()
        {
            ShopTabRefilled?.Invoke(m_GemShopItems);
            ShopTabRefilled?.Invoke(m_GoldShopItems);
            ShopTabRefilled?.Invoke(m_PotionShopItems);
        }

        // try to buy the item, pass the screen coordinate of the buy button
        public void OnTryBuyItem(ShopItemSO shopItemData, Vector2 screenPos)
        {
            if (shopItemData == null)
                return;

            // notify other objects we are trying to buy an item
            ShopItemPurchasing?.Invoke(shopItemData, screenPos);
        }
    }
}