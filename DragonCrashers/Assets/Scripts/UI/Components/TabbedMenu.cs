using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    // adapted from: https://docs.unity3d.com/2021.2/Documentation/Manual/UIE-create-tabbed-menu-for-runtime.html

    // This establishes a tabbed menu system for the UI document. This modifies the original to support multiple
    // tabbed menus within the same document. 

    [System.Serializable]
    public struct TabbedMenuIDs
    {
        // UXML selector for a clickable tab
        public string tabClassName;// = "tab";

        // UXML selector for currently selected tab 
        public string selectedTabClassName; //= "selected-tab";

        // UXML selector for content to hide
        public string unselectedContentClassName; // = "unselected-content";

        // use a basename to pair a tab with its content, e.g. 'name1-tab' matches 'name1-content'

        // suffix naming convention for tabs
        public string tabNameSuffix;// = "-tab";

        // suffix naming convention for content
        public string contentNameSuffix;// = "-content";

    }
    public class TabbedMenu : MonoBehaviour
    {
        [Tooltip("Defaults to current component unless specified")]
        [SerializeField] UIDocument m_Document;

        [Tooltip("VisualElement for TabbedMenu, defaults to document rootVisualElement if unspecified")]
        [SerializeField] string m_MenuElementName;

        TabbedMenuController m_Controller;
        VisualElement m_MenuElement;

        [Tooltip("Selectors and naming conventions for tabbed menu elements")]
        public TabbedMenuIDs m_TabbedMenuStrings;


        private void OnEnable()
        {
            if (m_Document == null)
                m_Document = GetComponent<UIDocument>();

            VisualElement root = m_Document.rootVisualElement;
            m_MenuElement = root.Q(m_MenuElementName);

            // create a new TabbedMenuController for a specific element (fall back to the entire tree if unspecified)
            m_Controller = (string.IsNullOrEmpty(m_MenuElementName) || m_MenuElement == null) ?
                new TabbedMenuController(root, m_TabbedMenuStrings) : new TabbedMenuController(m_MenuElement, m_TabbedMenuStrings);

            // set up the click events on the tab
            m_Controller.RegisterTabCallbacks();
        }

        // fill in default names for convenience - make these unique for each tabbed menu/UI
        void OnValidate()
        {
            if (string.IsNullOrEmpty(m_TabbedMenuStrings.tabClassName))
            {
                m_TabbedMenuStrings.tabClassName = "tab";
            }

            if (string.IsNullOrEmpty(m_TabbedMenuStrings.selectedTabClassName))
            {
                m_TabbedMenuStrings.selectedTabClassName = "selected-tab";
            }

            if (string.IsNullOrEmpty(m_TabbedMenuStrings.unselectedContentClassName))
            {
                m_TabbedMenuStrings.unselectedContentClassName = "unselected-content";
            }

            if (string.IsNullOrEmpty(m_TabbedMenuStrings.tabNameSuffix))
            {
                m_TabbedMenuStrings.tabNameSuffix = "-tab";
            }

            if (string.IsNullOrEmpty(m_TabbedMenuStrings.contentNameSuffix))
            {
                m_TabbedMenuStrings.contentNameSuffix = "-content";
            }
        }

        public void SelectFirstTab()
        {
            SelectFirstTab(m_MenuElement);
        }

        // select the first tab (specify any element below the tabbed menu, usually the menu screen)
        public void SelectFirstTab(VisualElement elementToQuery)
        {
            m_Controller?.SelectFirstTab(elementToQuery);
        }

        // select a specific tab by string ID
        public void SelectTab(string tabName)
        {
            
            m_Controller?.SelectTab(tabName);
        }

        public bool IsTabSelected(VisualElement visualElement)
        {
            if (m_Controller == null || visualElement == null)
            {
                return false;
            }

            return m_Controller.IsTabSelected(visualElement);
        }
    }
}

