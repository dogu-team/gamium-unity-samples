using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;

namespace UIToolkitDemo
{
    // non-UI logic for the CharScreen
    public class CharScreenController : MonoBehaviour
    {
        public static event Action<float> LevelUpdated;

        public static event Action Initialized;
        public static event Action<CharacterData> CharacterShown;
        public static event Action<EquipmentSO> GearUnequipped;
        public static event Action<EquipmentSO, int> GearSlotUpdated;
        public static event Action<CharacterData> CharacterAutoEquipped;
        public static event Action<List<CharacterData>> GearDataInitialized;
        public static event Action<CharacterData> LevelPotionUsed;


        [Tooltip("Characters to choose from.")]
        [SerializeField] List<CharacterData> m_Characters;

        [Tooltip("Parent transform for all character previews.")]
        [SerializeField] Transform m_previewTransform;

        [Header("Inventory")]
        [Tooltip("Check this option to allow only one type of gear (armor, weapon, etc.) per character.")]
        [SerializeField] bool m_UnequipDuplicateGearType;

        [Header("Level Up")]
        [SerializeField] [Tooltip("Controls playback of level up FX.")] PlayableDirector m_LevelUpPlayable;

        public CharacterData CurrentCharacter { get => m_Characters[m_CurrentIndex]; }

        int m_CurrentIndex;
        int m_ActiveGearSlot;


        void OnEnable()
        {
            CharacterData.LevelIncremented += OnLevelIncremented;

            CharScreen.ScreenEnabled += OnCharScreenStarted;
            CharScreen.ScreenDisabled += OnCharScreenEnded;
            CharScreen.NextCharacterSelected += SelectNextCharacter;
            CharScreen.LastCharacterSelected += SelectLastCharacter;
            CharScreen.InventoryOpened += OnInventoryOpened;
            CharScreen.GearAutoEquipped += OnGearAutoEquipped;
            CharScreen.GearUnequipped += OnGearUnequipped;
            CharScreen.LevelUpClicked += OnLevelUpClicked;

            InventoryScreen.GearSelected += OnGearSelected;

            InventoryScreenController.GearAutoEquipped += OnGearAutoEquipped;

            GameDataManager.CharacterLeveledUp += OnCharacterLeveledUp;

            SettingsScreen.ResetPlayerLevel += OnResetPlayerLevel;
        }

        void OnDisable()
        {
            CharacterData.LevelIncremented -= OnLevelIncremented;

            CharScreen.ScreenEnabled -= OnCharScreenStarted;
            CharScreen.ScreenDisabled -= OnCharScreenEnded;
            CharScreen.NextCharacterSelected -= SelectNextCharacter;
            CharScreen.LastCharacterSelected -= SelectLastCharacter;
            CharScreen.InventoryOpened -= OnInventoryOpened;
            CharScreen.GearAutoEquipped -= OnGearAutoEquipped;
            CharScreen.GearUnequipped -= OnGearUnequipped;
            CharScreen.LevelUpClicked -= OnLevelUpClicked;

            InventoryScreen.GearSelected -= OnGearSelected;

            InventoryScreenController.GearAutoEquipped -= OnGearAutoEquipped;

            GameDataManager.CharacterLeveledUp -= OnCharacterLeveledUp;

            SettingsScreen.ResetPlayerLevel -= OnResetPlayerLevel;
        }

        void Awake()
        {
            InitializeCharPreview();
        }

        void Start()
        {
            InitGearData();
            UpdateLevelMeter();
        }

        void UpdateView()
        {
            if (m_Characters.Count == 0)
                return;

            // show the Character Prefab
            CharacterShown?.Invoke(CurrentCharacter);

            // update the four gear slots
            UpdateGearSlots();

            UpdateLevelMeter();
        }

        // character preview methods
        public void SelectNextCharacter()
        {
            if (m_Characters.Count == 0)
                return;

            ShowCharacterPreview(false);

            m_CurrentIndex++;
            if (m_CurrentIndex >= m_Characters.Count)
                m_CurrentIndex = 0;

            // select next character from m_Characters and refresh the CharScreen
            UpdateView();
        }

        public void SelectLastCharacter()
        {
            if (m_Characters.Count == 0)
                return;

            ShowCharacterPreview(false);

            m_CurrentIndex--;
            if (m_CurrentIndex < 0)
                m_CurrentIndex = m_Characters.Count - 1;

            // select last character from m_Characters and refresh the CharScreen
            UpdateView();
        }

        // preview GameObject for each character
        void InitializeCharPreview()
        {
            foreach (CharacterData charData in m_Characters)
            {
                if (charData == null)
                {
                    Debug.LogWarning("CharScreenController.InitializeCharPreview Warning: Missing character data.");
                    continue;
                }
                GameObject previewInstance = Instantiate(charData.CharacterBaseData.characterVisualsPrefab);

                previewInstance.transform.SetParent(m_previewTransform);
                previewInstance.transform.localPosition = Vector3.zero;
                previewInstance.transform.localRotation = Quaternion.identity;
                previewInstance.transform.localScale = Vector3.one;
                charData.PreviewInstance = previewInstance;
                previewInstance.gameObject.SetActive(false);
            }

            Initialized?.Invoke();
       
        }

        void ShowCharacterPreview(bool state)
        {
            if (m_Characters.Count == 0)
                return;

            CharacterData currentCharacter = m_Characters[m_CurrentIndex];
            currentCharacter.PreviewInstance?.gameObject.SetActive(state);
            UpdateLevelMeter();
        }

        // assign starting gear for each character based on CharacterBaseSO
        void InitGearData()
        {
            // notify InventoryScreenController
            GearDataInitialized?.Invoke(m_Characters);
        }

        void UpdateGearSlots()
        {
            if (CurrentCharacter == null)
                return;

            for (int i = 0; i < CurrentCharacter.GearData.Length; i++)
            {
                GearSlotUpdated?.Invoke(CurrentCharacter.GearData[i], i);
            }
        }

        // removes a specific EquipmentType (helmet, shield/armor, weapon, gloves, boots) from a character;
        // use this to prevent duplicate gear types from appearing in the inventory.
        public void RemoveGearType(EquipmentType typeToRemove)
        {
            if (CurrentCharacter == null)
                return;

            // remove type from each character's inventory slot if found; notifies CharScreen
            for (int i = 0; i < CurrentCharacter.GearData.Length; i++)
            {
                if (CurrentCharacter.GearData[i] != null && CurrentCharacter.GearData[i].equipmentType == typeToRemove)
                {
                    GearUnequipped(CurrentCharacter.GearData[i]);
                    CurrentCharacter.GearData[i] = null;
                    GearSlotUpdated?.Invoke(null, i);
                }
            }
        }

        // character level methods
        int GetCharLevels()
        {
            int totalLevels = 0;
            foreach (CharacterData charData in m_Characters)
            {
                totalLevels += charData.CurrentLevel;
            }
            return totalLevels;
        }

        // update the upper left level meter
        void UpdateLevelMeter()
        {
            int totalLevels = GetCharLevels();
            LevelUpdated?.Invoke((float)totalLevels);
        }

        // event-handling methods

        private void OnResetPlayerLevel()
        {
            foreach (CharacterData charData in m_Characters)
            {
                charData.CurrentLevel = 0;
            }
            CharacterShown?.Invoke(CurrentCharacter);
            UpdateLevelMeter();
        }
        void OnCharScreenStarted()
        {
            UpdateView();
            ShowCharacterPreview(true);
        }

        void OnCharScreenEnded()
        {
            ShowCharacterPreview(false);
        }

        // click the level up button
        void OnLevelUpClicked()
        {
            // notify GameDataManager that we want to spend LevelUpPotion
            LevelPotionUsed?.Invoke(CurrentCharacter);
        }

        // update the character stats UI
        void OnLevelIncremented(CharacterData charData)
        {
            if (charData == CurrentCharacter)
            {
                CharacterShown?.Invoke(CurrentCharacter);
                UpdateLevelMeter();
            }
        }

        // success or failure when leveling up a character 
        void OnCharacterLeveledUp(bool didLevel)
        {
            if (didLevel)
            {
                //increment the Player Level
                CurrentCharacter.IncrementLevel();

                // playback the FX sequence
                m_LevelUpPlayable?.Play();
            }
        }

        // track the gear slot used to open the Inventory
        void OnInventoryOpened(int gearSlot)
        {
            m_ActiveGearSlot = gearSlot;
        }

        // handles gear selection from the Inventory Screen
        void OnGearSelected(EquipmentSO gearObject)
        {
            // if Slot already has an item, notify the InventoryScreenController and return it to the inventory
            if (CurrentCharacter.GearData[m_ActiveGearSlot] != null)
            {

                GearUnequipped?.Invoke(CurrentCharacter.GearData[m_ActiveGearSlot]);
                CurrentCharacter.GearData[m_ActiveGearSlot] = null;
            }

            // remove any duplicate EquipmentTypes (only permit one type of helmet, shield/armor, weapon, gloves, or boots)
            if (m_UnequipDuplicateGearType)
            {
                RemoveGearType(gearObject.equipmentType);
            }

            // set the Gear into the active slot
            CurrentCharacter.GearData[m_ActiveGearSlot] = gearObject;

            // notify CharScreen to update
            GearSlotUpdated?.Invoke(gearObject, m_ActiveGearSlot);
        }

        void OnGearUnequipped()
        {
            for (int i = 0; i < CurrentCharacter.GearData.Length; i++)
            {
                // if we currently have Equipment in one of the four gear slots, remove it
                if (CurrentCharacter.GearData[i] != null)
                {
                    // notifies the InventoryScreenController to unequip gear and update lists
                    GearUnequipped?.Invoke(CurrentCharacter.GearData[i]);

                    // clear the Equipment from the character's gear data
                    CurrentCharacter.GearData[i] = null;

                    // notify the CharScreen UI to update
                    GearSlotUpdated?.Invoke(null, i);
                }
            }
        }

        // send the current character to the InventoryScreenController to find gear for empty slots
        void OnGearAutoEquipped()
        {
            CharacterAutoEquipped?.Invoke(CurrentCharacter);
        }

        // fill in empty slots with gear
        void OnGearAutoEquipped(List<EquipmentSO> gearToEquip)
        {
            if (CurrentCharacter == null)
                return;

            int gearCounter = 0;

            for (int i = 0; i < CurrentCharacter.GearData.Length; i++)
            {
                if (CurrentCharacter.GearData[i] == null && gearCounter < gearToEquip.Count)
                {
                    CurrentCharacter.GearData[i] = gearToEquip[gearCounter];

                    // notify the CharScreen UI to update
                    GearSlotUpdated?.Invoke(gearToEquip[gearCounter], i);
                    gearCounter++;
                }
            }
        }
    }
}