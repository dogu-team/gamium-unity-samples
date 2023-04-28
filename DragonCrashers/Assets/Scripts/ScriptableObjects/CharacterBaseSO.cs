using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIToolkitDemo
{
    public enum CharacterClass
    {
        Paladin,
        Wizard,
        Barbarian,
        Necromancer
    }
    public enum Rarity
    {
        Common,
        Rare,
        Special,
        All, // for filtering
    }

    public enum AttackType
    {
        Melee,
        Magic,
        Ranged
    }

    // baseline data for a specific character

    [CreateAssetMenu(fileName ="Assets/Resources/GameData/Characters/CharacterGameData", menuName = "UIToolkitDemo/Character", order = 1)]
    public class CharacterBaseSO : ScriptableObject
    {
        public string characterName;
        public GameObject characterVisualsPrefab;
        public CharacterClass characterClass;
        public Rarity rarity;
        public AttackType attackType;

        public string bioTitle;
        [TextArea] public string bio;

        public float basePointsLife;
        public float basePointsDefense;
        public float basePointsAttack;
        public float basePointsAttackSpeed;
        public float basePointsSpecialAttack;
        public float basePointsCriticalHit;

        // skill1 unlocked at level 0, skill2 unlocked at level 3, skill3 unlocked at level 6
        public SkillSO skill1;
        public SkillSO skill2;
        public SkillSO skill3;

        // starting equipment (weapon, shield/armor, helmet, boots, gloves)
        public EquipmentSO defaultWeapon;
        public EquipmentSO defaultShieldAndArmor;
        public EquipmentSO defaultHelmet;
        public EquipmentSO defaultBoots;
        public EquipmentSO defaultGloves;

        void OnValidate()
        {
            if (defaultWeapon != null && defaultWeapon.equipmentType != EquipmentType.Weapon) 
                defaultWeapon = null;

            if (defaultShieldAndArmor != null && defaultShieldAndArmor.equipmentType != EquipmentType.Shield)
                defaultShieldAndArmor = null;

            if (defaultHelmet != null && defaultHelmet.equipmentType != EquipmentType.Helmet)
                defaultHelmet = null;

            if (defaultGloves != null && defaultGloves.equipmentType != EquipmentType.Gloves)
                defaultGloves = null;

            if (defaultBoots != null && defaultBoots.equipmentType != EquipmentType.Boots)
                defaultBoots = null;
        }
    }


}