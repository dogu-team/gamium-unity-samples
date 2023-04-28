using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UIToolkitDemo
{

    // adapted from: https://docs.unity3d.com/2021.2/Documentation/Manual/UIE-create-tabbed-menu-for-runtime.html

    public class TabbedMenuController
    {
        // event to notify other objects
        public static event Action TabSelected;

        // base VisualElement of the UI (e.g. MailScreen, CharScreen, ShopScreen)
        readonly VisualElement m_Root;

        // strings to query for VisualElements
        readonly TabbedMenuIDs m_IDs;

        // initialize the root Visual Element for reuse (constructor for non-Monobehaviour)
        public TabbedMenuController(VisualElement root, TabbedMenuIDs ids)
        {
            m_Root = root;
            m_IDs = ids;
        }

        // set up click events for tab buttons
        public void RegisterTabCallbacks()
        {
            // identify each tab 
            UQueryBuilder<VisualElement> tabs = GetAllTabs();

            // register the ClickTab event handler for each Visual Element
            tabs.ForEach(
                (t) =>
                {
                    t.RegisterCallback<ClickEvent>(OnTabClick);
                });
        }

        // process a click
        void OnTabClick(ClickEvent evt)
        {
            VisualElement clickedTab = evt.currentTarget as VisualElement;

            // if the clicked tab is not already selected, select the correct one
            if (!IsTabSelected(clickedTab))
            {
                // de-select any other tabs that are currently active
                GetAllTabs().Where(
                    (tab) => tab != clickedTab && IsTabSelected(tab)
                    ).ForEach(UnselectTab);

                // select the clicked tab
                SelectTab(clickedTab);
                AudioManager.PlayDefaultButtonSound();
            }
        }

        // return the corresponding content element for a given tab
        VisualElement FindContent(VisualElement tab)
        {
            return m_Root.Q(GetContentName(tab));
        }

        // return corresponding content name, e.g. name1-content for name1-tab
        string GetContentName(VisualElement tab)
        {
            return tab.name.Replace(m_IDs.tabNameSuffix, m_IDs.contentNameSuffix);
        }

        // locate all VisualElements that have the tab class name
        UQueryBuilder<VisualElement> GetAllTabs()
        {
            return m_Root.Query<VisualElement>(className: m_IDs.tabClassName);
        }

        // locate the first tab on a specific Visual Element (e.g. screen)
        public VisualElement GetFirstTab(VisualElement visualElement)
        {
            return visualElement.Query<VisualElement>(className: m_IDs.tabClassName).First();
        }

        public bool IsTabSelected(string tabName)
        {
            VisualElement tabElement = m_Root.Query<VisualElement>(className: m_IDs.tabClassName, name: tabName);
            return IsTabSelected(tabElement);
        }

        public bool IsTabSelected(VisualElement tab)
        {
            return tab.ClassListContains(m_IDs.selectedTabClassName);
        }

        void UnselectOtherTabs(VisualElement tab)
        {
            GetAllTabs().Where(
                (t) => t != tab && IsTabSelected(t)).
                ForEach(UnselectTab);
        }

        // select by name (used when opening a screen to a specific tab)
        public void SelectTab(string tabName)
        {
            if (string.IsNullOrEmpty(tabName))
            {
                return;
            }

            VisualElement namedTab = m_Root.Query<VisualElement>(tabName, className: m_IDs.tabClassName);

            if (namedTab == null)
            {
                Debug.Log("TabbedMenuController.SelectTab: invalid tab specified");
                return;
            }

            UnselectOtherTabs(namedTab);
            SelectTab(namedTab);
        }

        // selects a given tab, finds the corresponding content, and shows the content
        void SelectTab(VisualElement tab)
        {
            // highlight the tab
            tab?.AddToClassList(m_IDs.selectedTabClassName);

            // unhide the content
            VisualElement content = FindContent(tab);
            content?.RemoveFromClassList(m_IDs.unselectedContentClassName);

            // notify other objects 
            TabSelected?.Invoke();
        }

        // select the first tab of a given Visual Element (e.g. screen)
        public void SelectFirstTab(VisualElement visualElement)
        {
            VisualElement firstTab = GetFirstTab(visualElement);

            if (firstTab != null)
            {
                // de-select any other tabs that are currently active
                GetAllTabs().Where(
                    (tab) => tab != firstTab && IsTabSelected(tab)
                    ).ForEach(UnselectTab);

                // select just the first tab
                SelectTab(firstTab);
            }
        }

        // de-selects a specific tab, finds the corresponding content and hides the content
        void UnselectTab(VisualElement tab)
        {
            // unhighlight
            tab?.RemoveFromClassList(m_IDs.selectedTabClassName);

            // hide corresponding content
            VisualElement content = FindContent(tab);
            content?.AddToClassList(m_IDs.unselectedContentClassName);
        }
    }
}