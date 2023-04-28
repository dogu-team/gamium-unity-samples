using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIToolkitDemo
{
    // represents inventory gear
    public enum EquipmentType
    {
        Weapon,
        Helmet,
        Boots,
        Gloves,
        Shield,
        Accessories,
        All // for filtering
    }

    [CreateAssetMenu(fileName = "Assets/Resources/GameData/Equipment/EquipmentGameData", menuName = "UIToolkitDemo/Equipment", order = 2)]
    public class EquipmentSO : ScriptableObject
    {
        public string equipmentName;
        public EquipmentType equipmentType;
        public Rarity rarity;
        public int points;
        public Sprite sprite;
    }

}
