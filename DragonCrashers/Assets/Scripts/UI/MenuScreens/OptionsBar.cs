using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    public class OptionsBar : MenuScreen
    {
        // string IDs
        const string k_OptionsButton = "options-bar__button";
        const string k_ShopGemButton = "options-bar__gem-button";
        const string k_ShopGoldButton = "options-bar__gold-button";

        const string k_ShopGemTab = "shop-gem-shoptab";
        const string k_ShopGoldTab = "shop-gold-shoptab";

        const string k_GoldCount = "options-bar__gold-count";
        const string k_GemCount = "options-bar__gem-count";

        const float k_LerpTime = 0.6f;

        VisualElement m_OptionsButton;
        VisualElement m_ShopGemButton;
        VisualElement m_ShopGoldButton;
        Label m_GoldLabel;
        Label m_GemLabel;

        private void OnEnable()
        {
            GameDataManager.FundsUpdated += OnFundsUpdated;
        }

        private void OnDisable()
        {
            GameDataManager.FundsUpdated -= OnFundsUpdated;
        }

        // identify visual elements by name
        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            m_OptionsButton = m_Root.Q(k_OptionsButton);
            m_ShopGoldButton = m_Root.Q(k_ShopGoldButton);
            m_ShopGemButton = m_Root.Q(k_ShopGemButton);

            // shorthand equivalent to getting the first item named m_ShopGemButtonName:
            //      m_ShopGemButton = rootElement.Query<VisualElement>(m_ShopGemButtonName).First();

            m_GoldLabel = m_Root.Q<Label>(k_GoldCount);
            m_GemLabel = m_Root.Q<Label>(k_GemCount);
        }

        // set up button click events
        protected override void RegisterButtonCallbacks()
        {
            m_OptionsButton?.RegisterCallback<ClickEvent>(ShowOptionsScreen);
            m_ShopGemButton?.RegisterCallback<ClickEvent>(OpenGemShop);
            m_ShopGoldButton?.RegisterCallback<ClickEvent>(OpenGoldShop);
        }

        void ShowOptionsScreen(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            m_MainMenuUIManager?.ShowSettingsScreen();
            
        }

        void OpenGoldShop(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            m_MainMenuUIManager?.ShowShopScreen(k_ShopGoldTab);
        }

        void OpenGemShop(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            m_MainMenuUIManager?.ShowShopScreen(k_ShopGemTab);
        }

        public void SetGold(uint gold)
        {
            uint startValue = (uint) Int32.Parse(m_GoldLabel.text);
            StartCoroutine(LerpRoutine(m_GoldLabel, startValue, gold, k_LerpTime));
        }

        public void SetGems(uint gems)
        {
            uint startValue = (uint)Int32.Parse(m_GemLabel.text);
            StartCoroutine(LerpRoutine(m_GemLabel, startValue, gems, k_LerpTime));
        }

        void OnFundsUpdated(GameData gameData)
        {
            SetGold(gameData.gold);
            SetGems(gameData.gems);
        }

        // animated Label counter
        IEnumerator LerpRoutine(Label label, uint startValue, uint endValue, float duration)
        {
            float lerpValue = (float) startValue;
            float t = 0f;
            label.text = string.Empty;

            while (Mathf.Abs((float)endValue - lerpValue) > 0.05f)
            {
                t += Time.deltaTime / k_LerpTime;

                lerpValue = Mathf.Lerp(startValue, endValue, t);
                label.text = lerpValue.ToString("0");
                yield return null;
            }
            label.text = endValue.ToString();
        }
    }
}