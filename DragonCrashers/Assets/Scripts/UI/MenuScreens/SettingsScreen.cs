using System;
using UnityEngine;
using UnityEngine.UIElements;
using MyUILibrary;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace UIToolkitDemo
{

    // pairs a Theme StyleSheet with a seasonal variation/themes (e.g. Christmas, Halloween, etc.)
    [Serializable]
    public class ThemeSettings
    {
        public string theme;
        public ThemeStyleSheet tss;
    }

    // This controls general settings for the game. Many of these options are non-functional in this demo but
    // show how to sync data from a UI with the GameDataManager.
    public class SettingsScreen : MenuScreen
    {
        public static event Action ResetPlayerFunds;
        public static event Action ResetPlayerLevel;

        public static event Action SettingsShown;
        public static event Action<GameData> SettingsUpdated;

        [Space]
        [Tooltip("Define StyleSheets associated with each Theme. Each MenuScreen may have its own StyleSheet.")]
        [SerializeField] List<ThemeSettings> m_ThemeSettings;

        // string IDs
        const string k_PanelBackButton = "settings__panel-back-button";
        const string k_ResetLevelButton = "settings__social-button1";
        const string k_ResetFundsButton = "settings__social-button2";
        const string k_PlayerTextfield = "settings__player-textfield";
        const string k_ExampleToggle = "settings__email-toggle";
        const string k_ThemeDropdown = "settings__theme-dropdown";
        const string k_ExampleDropdown = "settings__language-dropdown";
        const string k_Slider1 = "settings__slider1";
        const string k_Slider2 = "settings__slider2";
        const string k_SlideToggle = "settings-notifications__toggle";
        const string k_SlideToggleOnLabel = "settings-notifications__label-on";
        const string k_SlideToggleOffLabel = "settings-notifications__label-off";
        const string k_RadioButtonGroup = "settings__graphics-radio-button-group";

        const string k_PanelActiveClass = "settings__panel";
        const string k_PanelInactiveClass = "settings__panel--inactive";
        const string k_LabelActiveClass = "slide-toggle__label";
        const string k_LabelInactiveClass = "slide-toggle__label--inactive";
        const string k_SettingsPanel = "settings__panel";

        // visual elements
        Button m_ResetLevelButton;
        Button m_ResetFundsButton;
        TextField m_PlayerTextfield;
        Toggle m_ExampleToggle;
        DropdownField m_ThemeDropdown;
        DropdownField m_ExampleDropdown;
        Slider m_MusicSlider;
        Slider m_SfxSlider;
        SlideToggle m_SlideToggle;
        Label m_OnLabel;
        Label m_OffLabel;
        RadioButtonGroup m_RadioButtonGroup;
        VisualElement m_PanelBackButton;

        // root node for transitions
        VisualElement m_Panel;

        // temp storage to send back to GameDataManager
        GameData m_SettingsData;

        void OnEnable()
        {
            // sets m_SettingsData
            SaveManager.GameDataLoaded += OnGameDataLoaded;
        }

        void OnDisable()
        {
            SaveManager.GameDataLoaded -= OnGameDataLoaded;
        }
        public override void ShowScreen()
        {
            base.ShowScreen();

            // add active style
            m_Panel.RemoveFromClassList(k_PanelInactiveClass);
            m_Panel.AddToClassList(k_PanelActiveClass);

            // notify GameDataManager
            SettingsShown?.Invoke();
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            m_PanelBackButton = m_Root.Q(k_PanelBackButton);
            m_ResetLevelButton = m_Root.Q<Button>(k_ResetLevelButton);
            m_ResetFundsButton = m_Root.Q<Button>(k_ResetFundsButton);
            m_PlayerTextfield = m_Root.Q<TextField>(k_PlayerTextfield);
            m_ExampleToggle = m_Root.Q<Toggle>(k_ExampleToggle);
            m_ThemeDropdown = m_Root.Q<DropdownField>(k_ThemeDropdown);
            m_ExampleDropdown = m_Root.Q<DropdownField>(k_ExampleDropdown);
            m_MusicSlider = m_Root.Q<Slider>(k_Slider1);
            m_SfxSlider = m_Root.Q<Slider>(k_Slider2);
            m_SlideToggle = m_Root.Q<SlideToggle>(k_SlideToggle);
            m_RadioButtonGroup = m_Root.Q<RadioButtonGroup>(k_RadioButtonGroup);
            m_OffLabel = m_Root.Q<Label>(k_SlideToggleOffLabel);
            m_OnLabel = m_Root.Q<Label>(k_SlideToggleOnLabel);
            m_Panel = m_Root.Q(k_SettingsPanel);
        }

        protected override void RegisterButtonCallbacks()
        {
            m_PanelBackButton?.RegisterCallback<ClickEvent>(ClosePanel);
            m_ResetLevelButton?.RegisterCallback<ClickEvent>(ResetLevel);
            m_ResetFundsButton?.RegisterCallback<ClickEvent>(ResetFunds);

            m_PlayerTextfield?.RegisterCallback<KeyDownEvent>(SetPlayerTextfield);
            m_ThemeDropdown?.RegisterValueChangedCallback(ChangeTheme);
            m_ThemeDropdown?.RegisterCallback<PointerDownEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_ExampleDropdown?.RegisterValueChangedCallback(ChangeExample);
            m_ExampleDropdown?.RegisterCallback<PointerDownEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_MusicSlider?.RegisterValueChangedCallback(ChangeMusicVolume);
            m_MusicSlider?.RegisterCallback<PointerDownEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_SfxSlider?.RegisterValueChangedCallback(ChangeSfxVolume);
            m_SfxSlider?.RegisterCallback<PointerDownEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_ExampleToggle?.RegisterValueChangedCallback(ChangeToggle);
            m_ExampleToggle?.RegisterCallback<ClickEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_SlideToggle?.RegisterValueChangedCallback(ChangeSlideToggle);
            m_SlideToggle?.RegisterCallback<ClickEvent>(evt => AudioManager.PlayDefaultButtonSound());

            m_RadioButtonGroup.RegisterCallback<ChangeEvent<int>>(ChangeRadioButton);
        }

        void ChangeSlideToggle(ChangeEvent<bool> evt)
        {
            ToggleLabel(m_OnLabel, m_SlideToggle.value);
            ToggleLabel(m_OffLabel, !m_SlideToggle.value);

            // non-functional setting for demo purposes
            m_SettingsData.isSlideToggled = evt.newValue;

            SettingsUpdated?.Invoke(m_SettingsData);
        }

        void ChangeToggle(ChangeEvent<bool> evt)
        {
            // non-functional setting for demo purposes
            m_SettingsData.isToggled = evt.newValue;

            // notify the GameDataManager
            SettingsUpdated?.Invoke(m_SettingsData);
        }

        void ChangeSfxVolume(ChangeEvent<float> evt)
        {
            m_SettingsData.sfxVolume = evt.newValue;

            // notify the GameDataManager
            SettingsUpdated?.Invoke(m_SettingsData);
        }

        void ChangeMusicVolume(ChangeEvent<float> evt)
        {
            m_SettingsData.musicVolume = evt.newValue;

            // notify the GameDataManager
            SettingsUpdated?.Invoke(m_SettingsData);
        }

        void ChangeExample(ChangeEvent<string> evt)
        {
            // non-functional setting for demo purposes
            m_SettingsData.dropdownSelection = evt.newValue;

            // notify the GameDataManager
            SettingsUpdated?.Invoke(m_SettingsData);
        }

        void ChangeRadioButton(ChangeEvent<int> evt)
        {
            AudioManager.PlayDefaultButtonSound();

            // non-functional setting for demo purposes
            m_SettingsData.buttonSelection = evt.newValue;

            // notify the GameDataManager
            SettingsUpdated?.Invoke(m_SettingsData);
        }

        void ResetLevel(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            ResetPlayerLevel?.Invoke();
        }

        void ResetFunds(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            ResetPlayerFunds?.Invoke();
        }
        void ClosePanel(ClickEvent evt)
        {
            m_Panel.RemoveFromClassList(k_PanelActiveClass);
            m_Panel.AddToClassList(k_PanelInactiveClass);

            AudioManager.PlayDefaultButtonSound();

            m_SettingsData.username = m_PlayerTextfield.text;
            
            SettingsUpdated?.Invoke(m_SettingsData);

            HideScreen();
        }

        // switch styles to trigger USS transition (fading text on/off e.g. SlideToggle)
        void ToggleLabel(Label label, bool value)
        {
            if (value)
            {
                label.AddToClassList(k_LabelActiveClass);
                label.RemoveFromClassList(k_LabelInactiveClass);
            }
            else
            {
                label.AddToClassList(k_LabelInactiveClass);
                label.RemoveFromClassList(k_LabelActiveClass);
            }
        }

        // change ThemeStyleSheets
        void ChangeTheme(ChangeEvent<string> evt)
        {
            ApplyTheme(evt.newValue);
            m_SettingsData.theme = evt.newValue;
            AudioManager.PlayAltButtonSound();
            SettingsUpdated?.Invoke(m_SettingsData);
        }

        ThemeStyleSheet GetThemeStyleSheet(string themeName)
        {
            ThemeSettings thisStyleSheet = m_ThemeSettings.Find(x => x.theme == themeName);

            return thisStyleSheet?.tss;
        }

        void ApplyTheme(string theme)
        {
            ThemeStyleSheet tss = GetThemeStyleSheet(theme);

            if (tss != null && m_Document != null && m_Document.panelSettings != null)
                m_Document.panelSettings.themeStyleSheet = tss;
        }

        // save the player name when hitting Return/Enter
        void SetPlayerTextfield(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Return && m_SettingsData != null)
            {
                m_SettingsData.username = m_PlayerTextfield.text;
                SettingsUpdated?.Invoke(m_SettingsData);
            }
        }

        // syncs saved data from the GameDataManager to the UI elements
        void OnGameDataLoaded(GameData gameData)
        {
            if (gameData == null)
                return;

            m_SettingsData = gameData;

            m_PlayerTextfield.value = gameData.username;
            m_ExampleDropdown.value = gameData.dropdownSelection;
            m_ThemeDropdown.value = gameData.theme;

            m_RadioButtonGroup.value = gameData.buttonSelection;

            m_MusicSlider.value = gameData.musicVolume;
            m_SfxSlider.value = gameData.sfxVolume;

            //Debug.Log("Settings Screen: Loading music volume: " + gameData.musicVolume);
            //Debug.Log("Settings Screen: Loading sfx volume: " + gameData.sfxVolume);

            m_SlideToggle.value = gameData.isSlideToggled;
            ToggleLabel(m_OnLabel, m_SlideToggle.value);
            ToggleLabel(m_OffLabel, !m_SlideToggle.value);

            ApplyTheme(m_ThemeDropdown.value);

            m_ExampleToggle.value = gameData.isToggled;

            SettingsUpdated?.Invoke(m_SettingsData);
        }
    }
}