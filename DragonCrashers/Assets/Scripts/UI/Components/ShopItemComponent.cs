using System;
using UnityEngine;
using UnityEngine.UIElements;


namespace UIToolkitDemo
{
    // represents one item in the shop
    public class ShopItemComponent
    {

        // notify the ShopScreenController to buy product (passes the ShopItem data + UI element screen position)
        public static event Action<ShopItemSO, Vector2> ShopItemClicked;

        // string IDs
        const string k_ParentContainer = "shop-item__parent-container";
        const string k_Description = "shop-item__description";
        const string k_ProductImage = "shop-item__product-image";

        const string k_Banner = "shop-item__banner";
        const string k_BannerLabel = "shop-item__banner-label";

        const string k_DiscountBadge = "shop-item__discount-badge";
        const string k_DiscountLabel = "shop-item__badge-text";
        const string k_DiscountSlash = "shop-item__discount-slash";

        const string k_ContentCurrency = "shop-item__content-currency";
        const string k_ContentValue = "shop-item__content-value";

        const string k_CostIcon = "shop-item__cost-icon";
        const string k_CostIconGroup = "shop-item__cost-icon-group";
        const string k_CostPrice = "shop-item__cost-price";

        const string k_DiscountIcon = "shop-item__discount-icon";
        const string k_DiscountIconGroup = "shop-item__discount-icon-group";
        const string k_DiscountPrice = "shop-item__discount-price";
        const string k_DiscountGroup = "shop-item__discount-group";

        const string k_BuyButton = "shop-item__buy-button";

        const string k_SizeNormalClass = "shop-item__size--normal";
        const string k_SizeWideClass = "shop-item__size--wide";

        // ScriptableObject pairing icons with currency/shop item types
        GameIconsSO m_GameIconsData;
        ShopItemSO m_ShopItemData;

        // visual elements
        Label m_Description;
        VisualElement m_ProductImage;
        VisualElement m_Banner;
        Label m_BannerLabel;
        VisualElement m_ContentCurrency;
        Label m_ContentValue;
        VisualElement m_CostIcon;
        Label m_Cost;
        VisualElement m_DiscountBadge;
        Label m_DiscountLabel;
        VisualElement m_DiscountSlash;
        VisualElement m_DiscountIcon;
        VisualElement m_DiscountGroup;
        VisualElement m_SizeContainer;
        Label m_DiscountCost;
        Button m_BuyButton;
        VisualElement m_CostIconGroup;
        VisualElement m_DiscountIconGroup;

        public ShopItemComponent(GameIconsSO gameIconsData, ShopItemSO shopItemData)
        {
            m_GameIconsData = gameIconsData;
            m_ShopItemData = shopItemData;
        }

        public void SetVisualElements(TemplateContainer shopItemElement)
        {
            // query the parts of the ShopItemElement
            m_SizeContainer = shopItemElement.Q(k_ParentContainer);
            m_Description = shopItemElement.Q<Label>(k_Description);
            m_ProductImage = shopItemElement.Q(k_ProductImage);
            m_Banner = shopItemElement.Q(k_Banner);
            m_BannerLabel = shopItemElement.Q<Label>(k_BannerLabel);
            m_DiscountBadge = shopItemElement.Q(k_DiscountBadge);
            m_DiscountLabel = shopItemElement.Q<Label>(k_DiscountLabel);
            m_DiscountSlash = shopItemElement.Q(k_DiscountSlash);
            m_ContentCurrency = shopItemElement.Q(k_ContentCurrency);
            m_ContentValue = shopItemElement.Q<Label>(k_ContentValue);
            m_CostIcon = shopItemElement.Q(k_CostIcon);
            m_Cost = shopItemElement.Q<Label>(k_CostPrice);
            m_DiscountIcon = shopItemElement.Q(k_DiscountIcon);
            m_DiscountGroup = shopItemElement.Q(k_DiscountGroup);
            m_DiscountCost = shopItemElement.Q<Label>(k_DiscountPrice);
            m_BuyButton = shopItemElement.Q<Button>(k_BuyButton);

            m_CostIconGroup = shopItemElement.Q(k_CostIconGroup);
            m_DiscountIconGroup = shopItemElement.Q(k_DiscountIconGroup);
        }

        // show the ScriptaboleObject data
        public void SetGameData(TemplateContainer shopItemElement)
        {
            if (m_GameIconsData == null)
            {
                Debug.LogWarning("ShopItemController SetGameData: missing GameIcons ScriptableObject data");
                return;
            }

            if (shopItemElement == null)
                return;

            // basic description and image
            m_Description.text = m_ShopItemData.itemName;
            m_ProductImage.style.backgroundImage = new StyleBackground(m_ShopItemData.sprite);

            // set up the promo banner
            m_Banner.style.display = (HasBanner(m_ShopItemData)) ? DisplayStyle.Flex : DisplayStyle.None;
            m_BannerLabel.style.display = (HasBanner(m_ShopItemData)) ? DisplayStyle.Flex : DisplayStyle.None;
            m_BannerLabel.text = m_ShopItemData.promoBannerText;

            // content value
            m_ContentCurrency.style.backgroundImage = new StyleBackground(m_GameIconsData.GetShopTypeIcon(m_ShopItemData.contentType));
            m_ContentValue.text = " " + m_ShopItemData.contentValue.ToString();

            FormatBuyButton();

            // use the oversize style if discounted
            if (IsDiscounted(m_ShopItemData))
            {
                m_SizeContainer.AddToClassList(k_SizeWideClass);
                m_SizeContainer.RemoveFromClassList(k_SizeNormalClass);
            }
            else
            {
                m_SizeContainer.AddToClassList(k_SizeNormalClass);
                m_SizeContainer.RemoveFromClassList(k_SizeWideClass);
            }
        }

        // format the cost and cost currency
        void FormatBuyButton()
        {
            string currencyPrefix = (m_ShopItemData.CostInCurrencyType == CurrencyType.USD) ? "$" : string.Empty;
            string decimalPlaces = (m_ShopItemData.CostInCurrencyType == CurrencyType.USD) ? "0.00" : "0";

            if (m_ShopItemData.cost > 0.00001f)
            {
                m_Cost.text = currencyPrefix + m_ShopItemData.cost.ToString(decimalPlaces);
                Sprite currencySprite = m_GameIconsData.GetCurrencyIcon(m_ShopItemData.CostInCurrencyType);

                m_CostIcon.style.backgroundImage = new StyleBackground(currencySprite);
                m_DiscountIcon.style.backgroundImage = new StyleBackground(currencySprite);

                m_CostIconGroup.style.display = (m_ShopItemData.CostInCurrencyType == CurrencyType.USD) ? DisplayStyle.None : DisplayStyle.Flex;
                m_DiscountIconGroup.style.display = (m_ShopItemData.CostInCurrencyType == CurrencyType.USD) ? DisplayStyle.None : DisplayStyle.Flex;

            }
            // if the cost is 0, mark the ShopItem as free and hide the cost currency
            else
            {
                m_CostIconGroup.style.display = DisplayStyle.None;
                m_DiscountIconGroup.style.display = DisplayStyle.None;
                m_Cost.text = "Free";
            }

            // disable/enabled, depending whether the item is discounted
            m_DiscountBadge.style.display = (IsDiscounted(m_ShopItemData)) ? DisplayStyle.Flex : DisplayStyle.None;
            m_DiscountLabel.text = m_ShopItemData.discount + "%";
            m_DiscountSlash.style.display = (IsDiscounted(m_ShopItemData)) ? DisplayStyle.Flex : DisplayStyle.None;
            m_DiscountGroup.style.display = (IsDiscounted(m_ShopItemData)) ? DisplayStyle.Flex : DisplayStyle.None;
            m_DiscountCost.text = currencyPrefix + (((100 - m_ShopItemData.discount) / 100f) * m_ShopItemData.cost).ToString(decimalPlaces);
        }

        bool IsDiscounted(ShopItemSO shopItem)
        {
            return (shopItem.discount > 0);
        }

        bool HasBanner(ShopItemSO shopItem)
        {
            return !string.IsNullOrEmpty(shopItem.promoBannerText);
        }

        public void RegisterCallbacks()
        {
            if (m_BuyButton == null)
                return;

            // store the cost/contents data in each button for later use
            m_BuyButton.userData = m_ShopItemData;
            m_BuyButton.RegisterCallback<ClickEvent>(BuyAction);
            m_BuyButton.RegisterCallback<PointerMoveEvent>(MovePointerEventHanlder);
        }

        void MovePointerEventHanlder(PointerMoveEvent evt)
        {
            // prevents accidental left-right movement of the mouse from dragging the parent Scrollview
            evt.StopImmediatePropagation();
        }

        void BuyAction(ClickEvent evt)
        {
            VisualElement clickedElement = evt.currentTarget as VisualElement;
            ShopItemSO shopItemData = clickedElement.userData as ShopItemSO;
            
            // starts a chain of events:

            //      ShopItemComponent (click the button) -->
            //      ShopController (buy an item) -->
            //      GameDataManager (verify funds)-->
            //      MagnetFXController (play effect on UI)

            // notify the ShopController (passes ShopItem data + UI Toolkit screen position)
            Vector2 screenPos = clickedElement.worldBound.center;

            ShopItemClicked?.Invoke(shopItemData, screenPos);

            AudioManager.PlayDefaultButtonSound();
        }
    }
}

