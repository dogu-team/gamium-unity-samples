using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;


namespace UIToolkitDemo
{
    // This is a base class for a functional unit of the Main Menu (a panel or a full-screen UI).

    // In this example, a MenuScreen could include:
    //      *full-screen UIs, e.g. HomeScreen, CharScreen, InfoScreen, ShopScreen, MailScreen, and OptionScreens
    //      *toolbars, e.g. the MenuBar (left) and OptionsBar (upper right)

    // A MenuScreen can be part of a larger UIDocument or use a separate UIDocument.

    public abstract class MenuScreen : MonoBehaviour
    {
        [Tooltip("String ID from the UXML for this menu panel/screen.")]
        [SerializeField] protected string m_ScreenName;

        [Header("UI Management")]
        [Tooltip("Set the Main Menu here explicitly (or get automatically from current GameObject).")]
        [SerializeField] protected MainMenuUIManager m_MainMenuUIManager;
        [Tooltip("Set the UI Document here explicitly (or get automatically from current GameObject).")]
        [SerializeField] protected UIDocument m_Document;

        // visual elements
        protected VisualElement m_Screen;
        protected VisualElement m_Root;

        public event Action ScreenStarted;
        public event Action ScreenEnded;

        //  UXML element name (defaults to the class name)
        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(m_ScreenName))
                m_ScreenName = this.GetType().Name;
        }

        protected virtual void Awake()
        {
            // set up MainMenuUIManager and UI Document
            if (m_MainMenuUIManager == null)
                m_MainMenuUIManager = GetComponent<MainMenuUIManager>();

            // default to current UIDocument if not set in Inspector
            if (m_Document == null)
                m_Document = GetComponent<UIDocument>();

            // alternately falls back to the MainMenu UI Document
            if (m_Document == null && m_MainMenuUIManager != null)
                m_Document = m_MainMenuUIManager.MainMenuDocument;

            if (m_Document == null)
            {
                Debug.LogWarning("MenuScreen " + m_ScreenName + ": missing UIDocument. Check Script Execution Order.");
                return;
            }
            else
            {
                SetVisualElements();
                RegisterButtonCallbacks();
            }
        }

        // The general workflow uses string IDs to query the VisualTreeAsset and find matching Visual Elements in the UXML.
        // Customize this for each MenuScreen subclass to identify any functional Visual Elements (buttons, controls, etc.).
        protected virtual void SetVisualElements()
        {
            // get a reference to the root VisualElement 
            if (m_Document != null)
                m_Root = m_Document.rootVisualElement;

            m_Screen = GetVisualElement(m_ScreenName);
        }

        // Once you have the VisualElements, you can add button events here, using the RegisterCallback functionality. 
        // This allows you to use a number of different events (ClickEvent, ChangeEvent, etc.)
        protected virtual void RegisterButtonCallbacks()
        {

        }

        public bool IsVisible()
        {
            if (m_Screen == null)
                return false;

            return (m_Screen.style.display == DisplayStyle.Flex);
        }

        // Toggle a UI on and off using the DisplayStyle. 
        public static void ShowVisualElement(VisualElement visualElement, bool state)
        {
            if (visualElement == null)
                return;

            visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
        }

        // returns an element by name
        public VisualElement GetVisualElement(string elementName)
        {
            if (string.IsNullOrEmpty(elementName) || m_Root == null)
                return null;

            // query and return the element
            return m_Root.Q(elementName);
        }

        public virtual void ShowScreen()
        {
            ShowVisualElement(m_Screen, true);
            ScreenStarted?.Invoke();
        }

        public virtual void HideScreen()
        {
            if (IsVisible())
            {
                ShowVisualElement(m_Screen, false);
                ScreenEnded?.Invoke();
            }
        }
    }
}