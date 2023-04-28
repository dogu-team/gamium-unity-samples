using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.Playables;

namespace UIToolkitDemo
{
    public class CharScreen : MenuScreen
    {
        // events
        public static event Action NextCharacterSelected;
        public static event Action LastCharacterSelected;
        public static event Action ScreenEnabled;
        public static event Action ScreenDisabled;
        public static event Action<int> InventoryOpened;
        public static event Action GearAutoEquipped;
        public static event Action GearUnequipped;
        public static event Action LevelUpClicked;

        // string IDs
        const string k_InventorySlot1Button = "char-inventory__slot1";
        const string k_InventorySlot2Button = "char-inventory__slot2";
        const string k_InventorySlot3Button = "char-inventory__slot3";
        const string k_InventorySlot4Button = "char-inventory__slot4";

        const string k_NextCharButton = "char__next-button";
        const string k_LastCharButton = "char__last-button";

        const string k_AutoEquipButton = "char__auto-equip-button";
        const string k_UnequipButton = "char__unequip-button";
        const string k_LevelUpButton = "char__level-up-button";
        const string k_LevelUpButtonVFX = "char__level-up-button-vfx";
        const string k_LevelUpButtonLabel = "char__level-up-button-label";

        const string k_CharacterLabel = "char__label";
        const string k_PotionsForNextLevel = "char__potion-to-advance";
        const string k_PotionCount = "char__potion-count";
        const string k_PowerLabel = "char__power-label";

        const string k_CharStatsPanel = "CharStatsPanel";
        const string k_ResourcePath = "GameData/GameIcons";

        const string k_LevelUpButtonInactiveClass = "char__level-up-button--inactive";
        const string k_LevelUpButtonClass = "char__level-up-button";
        const string k_ButtonLabelLargeClass = "button__label-large";
        const string k_ButtonLabelLargeInactiveClass = "button__label-large--inactive";

        Button[] m_GearSlots = new Button[4];

        Button m_LastCharButton;
        Button m_NextCharButton;
        Button m_AutoEquipButton;
        Button m_UnequipButton;
        Button m_LevelUpButton;

        Label m_CharacterLabel;
        Label m_PotionsForNextLevel;
        Label m_PotionCount;
        Label m_PowerLabel;
        Label m_LevelUpButtonLabel;

        VisualElement m_LevelUpButtonVFX;
        CharStatsWindow m_CharStatsWindow;
        VisualElement m_CharStatsContainer;
        TabbedMenu m_TabbedMenu;

        [Header("UI Assets")]
        [SerializeField] Sprite m_EmptyGearSlotSprite;
        [SerializeField] GameIconsSO m_GameIconsData;

        void OnEnable()
        {
            GameDataManager.LevelUpButtonEnabled += OnLevelUpButtonEnabled;
            GameDataManager.PotionsUpdated += OnPotionUpdated;

            CharScreenController.CharacterShown += OnCharacterShown;
            CharScreenController.Initialized += OnInitialized;
            CharScreenController.GearSlotUpdated += OnGearSlotUpdated;
        }

        void OnDisable()
        {
            GameDataManager.LevelUpButtonEnabled -= OnLevelUpButtonEnabled;
            GameDataManager.PotionsUpdated -= OnPotionUpdated;

            CharScreenController.CharacterShown -= OnCharacterShown;
            CharScreenController.Initialized -= OnInitialized;
            CharScreenController.GearSlotUpdated -= OnGearSlotUpdated;
        }

        protected override void Awake()
        {
            base.Awake();

            if (m_GameIconsData == null)
                m_GameIconsData = Resources.Load<GameIconsSO>(k_ResourcePath);

            if (m_TabbedMenu == null)
                m_TabbedMenu = GetComponent<TabbedMenu>();
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            m_GearSlots[0] = m_Root.Q<Button>(k_InventorySlot1Button);
            m_GearSlots[1] = m_Root.Q<Button>(k_InventorySlot2Button);
            m_GearSlots[2] = m_Root.Q<Button>(k_InventorySlot3Button);
            m_GearSlots[3] = m_Root.Q<Button>(k_InventorySlot4Button);

            m_NextCharButton = m_Root.Q<Button>(k_NextCharButton);
            m_LastCharButton = m_Root.Q<Button>(k_LastCharButton);

            m_AutoEquipButton = m_Root.Q<Button>(k_AutoEquipButton);
            m_UnequipButton = m_Root.Q<Button>(k_UnequipButton);
            m_LevelUpButton = m_Root.Q<Button>(k_LevelUpButton);
            m_LevelUpButtonVFX = m_Root.Q<VisualElement>(k_LevelUpButtonVFX);
            m_LevelUpButtonLabel = m_Root.Q<Label>(k_LevelUpButtonLabel);

            m_CharacterLabel = m_Root.Q<Label>(k_CharacterLabel);
            m_PotionCount = m_Root.Q<Label>(k_PotionCount);
            m_PotionsForNextLevel = m_Root.Q<Label>(k_PotionsForNextLevel);
            m_PowerLabel = m_Root.Q<Label>(k_PowerLabel);

            m_CharStatsContainer = m_Root.Q<VisualElement>(k_CharStatsPanel);
        }

        protected override void RegisterButtonCallbacks()
        {
            m_GearSlots[0]?.RegisterCallback<ClickEvent>(ShowInventory);
            m_GearSlots[1]?.RegisterCallback<ClickEvent>(ShowInventory);
            m_GearSlots[2]?.RegisterCallback<ClickEvent>(ShowInventory);
            m_GearSlots[3]?.RegisterCallback<ClickEvent>(ShowInventory);

            m_NextCharButton?.RegisterCallback<ClickEvent>(GoToNextCharacter);
            m_LastCharButton?.RegisterCallback<ClickEvent>(GoToLastCharacter);

            m_AutoEquipButton?.RegisterCallback<ClickEvent>(AutoEquipSlots);
            m_UnequipButton?.RegisterCallback<ClickEvent>(UnequipSlots);
            m_LevelUpButton?.RegisterCallback<ClickEvent>(LevelUpCharacter);
        }

        public override void ShowScreen()
        {
            base.ShowScreen();
            m_TabbedMenu?.SelectFirstTab(m_CharStatsContainer);
            ScreenEnabled?.Invoke();
        }

        public override void HideScreen()
        {
            base.HideScreen();
            ScreenDisabled?.Invoke();
        }

        void LevelUpCharacter(ClickEvent evt)
        {
            LevelUpClicked?.Invoke();
        }

        // notify CharScreenController to unequip all gear
        public void UnequipSlots(ClickEvent evt)
        {
            AudioManager.PlayAltButtonSound();
            GearUnequipped?.Invoke();
        }

        // notify CharScreenController to equip best gear available in empty slots
        public void AutoEquipSlots(ClickEvent evt)
        {
            AudioManager.PlayAltButtonSound();
            GearAutoEquipped?.Invoke();
        }

        void GoToLastCharacter(ClickEvent evt)
        {
            AudioManager.PlayAltButtonSound();
            LastCharacterSelected?.Invoke();
        }

        void GoToNextCharacter(ClickEvent evt)
        {
            AudioManager.PlayAltButtonSound();
            NextCharacterSelected?.Invoke();
        }

        // open the inventory screen when clicking on a gear slot
        void ShowInventory(ClickEvent evt)
        {
            VisualElement clickedElement = evt.target as VisualElement;

            char slotNumber = clickedElement.name[clickedElement.name.Length - 1];
            int slot = (int) Char.GetNumericValue(slotNumber) - 1;

            AudioManager.PlayDefaultButtonSound();
            m_MainMenuUIManager?.ShowInventoryScreen();
            InventoryOpened?.Invoke(slot);
        }

        public void UpdateCharacterStats(CharacterData characterToShow)
        {

            if (m_CharStatsContainer == null)
            {
                Debug.LogWarning("CharScreen.ShowCharacter: missing CharStatsPanel element");
                return;
            }

            // update character statistics UI
            if (m_GameIconsData == null)
            {
                Debug.LogWarning("CharScreen.ShowCharacter: missing GameIcons ScriptableObject data");
                return;
            }

            // create the CharStatsWindow if it doesn't exist already; otherwise, just update it
            if (m_CharStatsWindow == null)
            {
                m_CharStatsWindow = new CharStatsWindow(m_GameIconsData, characterToShow);
                m_CharStatsWindow.SetVisualElements(m_CharStatsContainer);
                m_CharStatsWindow?.SetGameData();
                m_CharStatsWindow.RegisterCallbacks();
            }
            else
            {
                m_CharStatsWindow.UpdateWindow(characterToShow, m_CharStatsContainer);
            }
        }

        void UpdatePotionCountLabel()
        {
            if (m_PotionsForNextLevel == null)
                return;

            string potionsForNextLevelString = m_PotionsForNextLevel.text.TrimStart('/');

            if (potionsForNextLevelString != string.Empty)
            {
                int potionsForNextLevel = Int32.Parse(potionsForNextLevelString);
                int potionsCount = Int32.Parse(m_PotionCount.text);
                m_PotionCount.style.color = (potionsForNextLevel > potionsCount) ? new Color(0.88f, .36f, 0f): new Color(0.81f, 0.94f, 0.48f);
            }
        }
        // event handling methods
        void OnInitialized()
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        public void OnCharacterShown(CharacterData characterToShow)
        {
            if (characterToShow == null)
                return;

            if (m_CharacterLabel != null)
                m_CharacterLabel.text = characterToShow.CharacterBaseData.characterName;

            if (m_PowerLabel != null)
                m_PowerLabel.text = characterToShow.GetCurrentPower().ToString();

            if (m_PotionsForNextLevel != null)
            {
                m_PotionsForNextLevel.text = "/" + characterToShow.GetXPForNextLevel().ToString();

                UpdatePotionCountLabel();
            }

            UpdateCharacterStats(characterToShow);

            characterToShow.PreviewInstance?.gameObject.SetActive(true);
        }

        void OnPotionUpdated(GameData gameData)
        {
            if (m_PotionCount == null)
                return;

            if (gameData == null)
                return;

            m_PotionCount.text = gameData.levelUpPotions.ToString();
            UpdatePotionCountLabel();
        }

        // shows the correct sprite for each gear slot
        void OnGearSlotUpdated(EquipmentSO gearData, int slotToUpdate)
        {
            Button activeSlot = m_GearSlots[slotToUpdate];

            // plus symbol is the first child of char-inventory__slot-n
            VisualElement addSymbol = activeSlot.ElementAt(0);

            // background sprite is the second child of char-inventory__slot-n
            VisualElement gearElement = activeSlot.ElementAt(1);

            if (gearData == null)
            {
                if (gearElement != null)
                    gearElement.style.backgroundImage = new StyleBackground(m_EmptyGearSlotSprite);

                if (addSymbol != null)
                    addSymbol.style.display = DisplayStyle.Flex;
            }
            else
            {
                if (gearElement != null)
                    gearElement.style.backgroundImage = new StyleBackground(gearData.sprite);

                if (addSymbol != null)
                    addSymbol.style.display = DisplayStyle.None;
            }
        }

        // toggle LevelUp VFX and button color based on available potions
        void OnLevelUpButtonEnabled(bool state)
        {
            if (m_LevelUpButtonVFX == null || m_LevelUpButton == null)
                return;

            m_LevelUpButtonVFX.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;

            // place in front of the button (a sibling in the UXML hierarchy)
            m_LevelUpButtonVFX.PlaceInFront(m_LevelUpButton);

            if (state)
            {
                m_LevelUpButton?.AddToClassList(k_LevelUpButtonClass);
                m_LevelUpButton?.RemoveFromClassList(k_LevelUpButtonInactiveClass);

                m_LevelUpButtonLabel?.AddToClassList(k_ButtonLabelLargeClass);
                m_LevelUpButtonLabel?.RemoveFromClassList(k_ButtonLabelLargeInactiveClass);
            }
            else
            {
                m_LevelUpButton?.AddToClassList(k_LevelUpButtonInactiveClass);
                m_LevelUpButton?.RemoveFromClassList(k_LevelUpButtonClass);

                m_LevelUpButtonLabel?.AddToClassList(k_ButtonLabelLargeInactiveClass);
                m_LevelUpButtonLabel?.RemoveFromClassList(k_ButtonLabelLargeClass);
            }
        }
    }
}