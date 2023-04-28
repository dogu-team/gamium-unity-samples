using System;
using UnityEngine;
using UnityEngine.UIElements;
using ChartLibrary;
using System.Collections;

namespace UIToolkitDemo
{
    // shows the menu buttons (bottom left) and level experience meter (top left)
    public class MenuBar : MenuScreen
    {

        // menu button (bottom)
        const string k_HomeScreenMenuButton = "menu__home-button";
        const string k_CharScreenMenuButton = "menu__char-button";
        const string k_InfoScreenMenuButton = "menu__info-button";
        const string k_ShopScreenMenuButton = "menu__shop-button";
        const string k_MailScreenMenuButton = "menu__mail-button";

        const string k_MenuMarker = "menu__current-marker";

        //// base node to apply ThemedStyleSheet
        //const string k_ThemeRootElement = "menu__background";

        // classes/selectors for toggling between active and inactive states
        const string k_LabelInactiveClass = "menu__label";
        const string k_LabelActiveClass = "menu__label--active";
        const string k_IconInactiveClass = "menu__icon";
        const string k_IconActiveClass = "menu__icon--active";

        const string k_ButtonInactiveClass = "menu__button";
        const string k_ButtonActiveClass = "menu__button--active";

        // marker movement
        const int k_MoveTime = 150;
        const float k_Spacing = 100f;
        const float k_yOffset = -8f;

        // UI Buttons
        Button m_HomeScreenMenuButton;
        Button m_CharScreenMenuButton;
        Button m_InfoScreenMenuButton;
        Button m_ShopScreenMenuButton;
        Button m_MailScreenMenuButton;

        VisualElement m_MenuMarker;

        // root element to apply theme
        VisualElement m_ThemeRootElement;

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            m_HomeScreenMenuButton = m_Root.Q<Button>(k_HomeScreenMenuButton);
            m_CharScreenMenuButton = m_Root.Q<Button>(k_CharScreenMenuButton);
            m_InfoScreenMenuButton = m_Root.Q<Button>(k_InfoScreenMenuButton);
            m_ShopScreenMenuButton = m_Root.Q<Button>(k_ShopScreenMenuButton);
            m_MailScreenMenuButton = m_Root.Q<Button>(k_MailScreenMenuButton);

            m_MenuMarker = m_Root.Q(k_MenuMarker);

            //m_ThemeRootElement = m_Root.Q(k_ThemeRootElement);
        }

        protected override void RegisterButtonCallbacks()
        {
            base.RegisterButtonCallbacks();

            // register action when each button is clicked
            m_HomeScreenMenuButton?.RegisterCallback<ClickEvent>(ShowHomeScreen);
            m_CharScreenMenuButton?.RegisterCallback<ClickEvent>(ShowCharScreen);
            m_InfoScreenMenuButton?.RegisterCallback<ClickEvent>(ShowInfoScreen);
            m_ShopScreenMenuButton?.RegisterCallback<ClickEvent>(ShowShopScreen);
            m_MailScreenMenuButton?.RegisterCallback<ClickEvent>(ShowMailScreen);

            // waits for interface to build (GeometryChangedEvent), otherwise marker can miss target
            m_MenuMarker?.RegisterCallback<GeometryChangedEvent>(GeometryChangedEventHandler);
        }

        void GeometryChangedEventHandler(GeometryChangedEvent evt)
        {
            ActivateButton(m_HomeScreenMenuButton);
            MoveActiveMarker(m_HomeScreenMenuButton);
        }

        void ShowHomeScreen(ClickEvent evt)
        {
            ActivateButton(m_HomeScreenMenuButton);
            m_MainMenuUIManager?.ShowHomeScreen();
            ClickMarker(evt);
        }

        void ShowCharScreen(ClickEvent evt)
        {
            ActivateButton(m_CharScreenMenuButton);
            m_MainMenuUIManager?.ShowCharScreen();
            ClickMarker(evt);

        }
        void ShowInfoScreen(ClickEvent evt)
        {
            ActivateButton(m_InfoScreenMenuButton);
            m_MainMenuUIManager?.ShowInfoScreen();
            ClickMarker(evt);

        }
        public void ShowShopScreen(ClickEvent evt)
        {
            m_MainMenuUIManager?.ShowShopScreen();
            ActivateButton(m_ShopScreenMenuButton);
            ClickMarker(evt);

        }
        public void ShowShopScreen()
        {
            m_MainMenuUIManager?.ShowShopScreen();
            ActivateButton(m_ShopScreenMenuButton);
            MoveActiveMarker(m_ShopScreenMenuButton);

        }
        void ShowMailScreen(ClickEvent evt)
        {
            m_MainMenuUIManager?.ShowMailScreen();
            ActivateButton(m_MailScreenMenuButton);
            ClickMarker(evt);

        }

        void ActivateButton(Button menuButton)
        {
            if (menuButton == null)
                return;

            HighlightElement(menuButton, k_ButtonInactiveClass, k_ButtonActiveClass, m_Root);

            // enable the label and disable others
            Label label = menuButton.Q<Label>(className: k_LabelInactiveClass);
            HighlightElement(label, k_LabelInactiveClass, k_LabelActiveClass, m_Root);

            // enable the icon and disable others
            VisualElement icon = menuButton.Q<VisualElement>(className: k_IconInactiveClass);
            HighlightElement(icon, k_IconInactiveClass, k_IconActiveClass, m_Root);

        }

        void ClickMarker(ClickEvent evt)
        {
            // move the marker when we click the target VisualElement directly
            if (evt.propagationPhase == PropagationPhase.AtTarget)
            {
                MoveActiveMarker(evt.target as VisualElement);
            }
            AudioManager.PlayDefaultButtonSound();
        }

        void MoveActiveMarker(VisualElement targetElement)
        {
            // world space position
            Vector2 targetInWorldSpace = targetElement.parent.LocalToWorld(targetElement.layout.position);

            // convert to local space of menu marker's parent
            Vector3 targetInRootSpace = m_MenuMarker.parent.WorldToLocal(targetInWorldSpace);

            // difference between image sizes
            Vector3 offset = new Vector3(0f, targetElement.parent.layout.height - targetElement.layout.height + k_yOffset, 0f);

            Vector3 newPosition = targetInRootSpace - offset;

            // add extra animation time if moving more than one space 
            Vector3 delta = m_MenuMarker.transform.position - newPosition;
            float distanceInPixels = Mathf.Abs(delta.y / k_Spacing);

            int duration = Mathf.Clamp((int)distanceInPixels * k_MoveTime, k_MoveTime, k_MoveTime * 4);

            m_MenuMarker?.experimental.animation.Position(targetInRootSpace - offset, duration);
        }

        // toggles between a highlighted/active class and an inactive class
        void HighlightElement(VisualElement visualElem, string inactiveClass, string activeClass, VisualElement root)
        {
            if (visualElem == null)
                return;

            VisualElement currentSelect = root.Query<VisualElement>(className: activeClass);

            if (currentSelect == visualElem)
            {
                return;
            }

            // de-highlight whatever is currently active
            currentSelect?.RemoveFromClassList(activeClass);
            currentSelect?.AddToClassList(inactiveClass);

            visualElem.RemoveFromClassList(inactiveClass);
            visualElem.AddToClassList(activeClass);
        }
    }
}