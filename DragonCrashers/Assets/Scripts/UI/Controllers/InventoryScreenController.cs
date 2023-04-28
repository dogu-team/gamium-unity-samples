using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UIToolkitDemo
{
    // non-UI logic for the Inventory Screen
    public class InventoryScreenController : MonoBehaviour
    {
        // events
        public static Action InventorySetup;
        public static Action<List<EquipmentSO>> InventoryUpdated;
        public static Action<List<EquipmentSO>> GearAutoEquipped;

        [Tooltip("Path within the Resources folders for Equipment ScriptableObjects.")]
        [SerializeField] string m_ResourcePath = "GameData/Equipment";

        [SerializeField] List<EquipmentSO> m_AllGear;

        List<EquipmentSO> m_UnequippedGear = new List<EquipmentSO>();
        List<EquipmentSO> m_EquippedGear = new List<EquipmentSO>();

        void OnEnable()
        {
            InventoryScreen.ScreenEnabled += OnInventoryScreenEnabled;
            InventoryScreen.GearFiltered += OnGearFiltered;
            InventoryScreen.GearSelected += OnGearEquipped;

            CharScreenController.GearDataInitialized += OnGearInitialized;
            CharScreenController.GearUnequipped += OnGearUnequipped;
            CharScreenController.CharacterAutoEquipped += OnCharacterAutoEquipped;
        }

        void OnDisable()
        {
            InventoryScreen.ScreenEnabled -= OnInventoryScreenEnabled;
            InventoryScreen.GearFiltered -= OnGearFiltered;
            InventoryScreen.GearSelected -= OnGearEquipped;

            CharScreenController.GearDataInitialized -= OnGearInitialized;
            CharScreenController.GearUnequipped -= OnGearUnequipped;
            CharScreenController.CharacterAutoEquipped -= OnCharacterAutoEquipped;
        }

        void Awake()
        {
            m_AllGear = LoadGearData();
            m_UnequippedGear = SortGearList(m_AllGear);
        }

        // load the Equipment ScriptableObject data from the Resources directory (default = Resources/GameData/Equipment)
        List<EquipmentSO> LoadGearData()
        {
            m_AllGear.AddRange(Resources.LoadAll<EquipmentSO>(m_ResourcePath));
            return SortGearList(m_AllGear);
        }

        // sort and display unequipped gear
        void UpdateInventory(List<EquipmentSO> gearToShow)
        {
            m_UnequippedGear = SortGearList(m_UnequippedGear);
            InventoryUpdated?.Invoke(gearToShow);
        }

        // returns a new list, filtered by specific Rarity and EquipmentType
        List<EquipmentSO> FilterGearList(List<EquipmentSO> gearList, Rarity rarity, EquipmentType gearType)
        {
            List<EquipmentSO> filteredList = gearList;

            if (rarity != Rarity.All)
            {
                filteredList = filteredList.Where(x => x.rarity == rarity).ToList();
            }

            if (gearType != EquipmentType.All)
            {
                filteredList = filteredList.Where(x => x.equipmentType == gearType).ToList();
            }

            return filteredList;
        }
        // sorts a list of gear by Rarity and then EquipmentType
        List<EquipmentSO> SortGearList(List<EquipmentSO> gearToSort)
        {
            if (gearToSort.Count <= 1)
                return gearToSort;

            // sort by Rarity, then by EquipmentType
            return gearToSort.OrderBy(x => x.rarity).
                ThenBy(x => x.equipmentType).
                ToList();
        }

        // returns the rarest gear of a specific Equipment Type 
        public EquipmentSO GetBestByType(EquipmentType gearType, List<EquipmentSO> gearList)
        {
            EquipmentSO gearToReturn = null;
            foreach (EquipmentSO g in gearList)
            {
                if (g.equipmentType != gearType)
                {
                    continue;
                }

                if (gearToReturn == null || (int)g.rarity > (int)gearToReturn.rarity)
                {
                    gearToReturn = g;
                }
            }

            return gearToReturn;
        }

        // return list of a character's unused Equipment Types
        public List<EquipmentType> GetUnusedTypes(CharacterData charData)
        {
            // auto-equip EquipmentTypes
            List<EquipmentType> unusedTypes = new List<EquipmentType>() { EquipmentType.Weapon, EquipmentType.Shield, EquipmentType.Helmet, EquipmentType.Gloves, EquipmentType.Boots };

            for (int i = 0; i < charData.GearData.Length; i++)
            {
                if (charData.GearData[i] != null)
                {
                    unusedTypes.Remove(charData.GearData[i].equipmentType);
                }
            }
            //DebugEquipmentTypes(unusedTypes, "Unused types: ");
            return unusedTypes;
        }

        // returns a list of best available gear to fill in a character's empty gear slots
        public List<EquipmentSO> GetUnusedGear(CharacterData charData)
        {

            List<EquipmentSO> unusedGear = new List<EquipmentSO>();
            List<EquipmentType> unusedTypes = GetUnusedTypes(charData);

            // count the number of empty slots in character gear
            int slotsToFill = charData.GearData.Count(s => s == null);

            for (int i = 0; i < unusedTypes.Count; i++)
            {
                if (slotsToFill <= 0)
                    break;

                EquipmentSO nextGear = GetBestByType(unusedTypes[i], m_UnequippedGear);
                if (nextGear != null)
                {
                    unusedGear.Add(nextGear);
                    slotsToFill--;
                }
            }

            return unusedGear;
        }

        // fill in the next empty GearData slot on the CharacterData
        void EquipGear(CharacterData charData, EquipmentSO gearToEquip)
        {
            if (gearToEquip == null)
            {
                return;
            }

            if (!m_UnequippedGear.Contains(gearToEquip))
            {
                //Debug.Log("Default gear: " + gearToEquip.equipmentName + " is not available for " + charData.CharacterBaseData.characterName + ". Skipping...");
                return;
            }

            for (int i = 0; i < charData.GearData.Length; i++)
            {
                if (charData.GearData[i] == null)
                {
                    charData.GearData[i] = gearToEquip;
                    m_UnequippedGear?.Remove(gearToEquip);
                    m_EquippedGear?.Add(gearToEquip);
                    break;
                }
            }
        }

        // load up a Character's preferred Gear (from base data)
        private void SetupDefaultGear(CharacterData charData)
        {
            //Debug.Log("Setting up Default Gear for " + charData.CharacterBaseData.characterName);

            // equip default gear from ScriptableObject base data
            EquipGear(charData, charData.CharacterBaseData.defaultWeapon);
            EquipGear(charData, charData.CharacterBaseData.defaultShieldAndArmor);
            EquipGear(charData, charData.CharacterBaseData.defaultHelmet);
            EquipGear(charData, charData.CharacterBaseData.defaultGloves);
            EquipGear(charData, charData.CharacterBaseData.defaultBoots);
        }

        // event handling methods
        private void OnInventoryScreenEnabled()
        {
            UpdateInventory(m_UnequippedGear);
        }

        void OnGearFiltered(Rarity rarity, EquipmentType gearType)
        {
            List<EquipmentSO> filteredGear = new List<EquipmentSO>();

            filteredGear = FilterGearList(m_UnequippedGear, rarity, gearType);

            UpdateInventory(filteredGear);
        }

        // update the inventory if we equip the gear from the UI
        void OnGearEquipped(EquipmentSO gearToEquip)
        {
            if (gearToEquip == null)
                return;

            // temp remove from the unequipped list and reload the gear data
            m_UnequippedGear?.Remove(gearToEquip);

            m_EquippedGear?.Add(gearToEquip);

            // notify InventoryScreen to refresh
            InventoryUpdated?.Invoke(m_UnequippedGear);
        }

        // update and sort the inventory after unequipping
        void OnGearUnequipped(EquipmentSO gearToUnequip)
        {
            if (gearToUnequip == null)
                return;

            // return unequipped item to Inventory
            m_UnequippedGear?.Add(gearToUnequip);
            m_EquippedGear?.Remove(gearToUnequip);

            // sort by Rarity, EquipmentType
            m_UnequippedGear = SortGearList(m_UnequippedGear);
        }

        // search unused inventory to fill character's empty gear slots
        void OnCharacterAutoEquipped(CharacterData charData)
        {
            if (charData == null)
                return;

            // get best gear from unused inventory
            List<EquipmentSO> gearToEquip = GetUnusedGear(charData);

            // categorize each as equipped/unequipped
            foreach (EquipmentSO gearData in gearToEquip)
            {
                OnGearEquipped(gearData);
            }

            // notify CharScreenController to update
            GearAutoEquipped?.Invoke(gearToEquip);
        }

        // set up default gear data
        void OnGearInitialized(List<CharacterData> allCharData)
        {
            // set up default Equipment SO in each character
            foreach (CharacterData charData in allCharData)
            {
                SetupDefaultGear(charData);
            }

            // notify InventoryScreen to set up UI
            InventorySetup?.Invoke();
        }
    }
}
