using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Util;
using Object = UnityEngine.Object;

namespace Data.Static
{
    [Serializable]
    public abstract class VariadicItemInfoBase : EnumbaseClass<VariadicItemType>
    {
        public string nickname;
        [TextArea] public string description;
        public Sprite icon;

        public abstract void OnValidate();
    }


    [Serializable]
    public class EquipmentVariadicItemInfo : VariadicItemInfoBase
    {
        public override VariadicItemType GetDataType() => VariadicItemType.Equipment;

        public override void OnValidate()
        {
            if (!EditorConditions.IsValidatable()) return;
            
            ChartValue.ForceXMax(prices, upgradeMax);
            ChartValue.ForceXMax(upgradeFailChances, upgradeMax);
            foreach (var stat in stats)
            {
                ChartValue.ForceXMax(stat.value, upgradeMax);
            }
        }

        public EquipmentPosition equipmentPosition;
        public uint upgradeMax;

        public ChartValue prices = new ChartValue();
        public ChartValue upgradeFailChances = new ChartValue();

        public SizeFixedEnumArray<Stat.StatType, ChartValue>.Entry[] stats =
            new SizeFixedEnumArray<Stat.StatType, ChartValue>.Entry[] { };
#if UNITY_EDITOR
        [EditorButton(typeof(EquipmentVariadicItemInfo), "OnGenerate")] [SerializeField]
        private bool onGenerate;

        private static void OnGenerate(Object obj)
        {
            var variadicItemInfo = obj as VariadicItemInfo;
            var equipmentVariadicItemInfo = variadicItemInfo.value as EquipmentVariadicItemInfo;
            Debug.Log("action");
            for (uint i = 0; i < equipmentVariadicItemInfo.upgradeMax; i++)
            {
                var itemInfo = ScriptableObject.CreateInstance<ItemInfo>();
                itemInfo.type = ItemType.Equipment;
                itemInfo.value = ItemInfoRegistry.instance.Create(ItemType.Equipment);
                var equipmentInfo = itemInfo.value as EquipmentItemInfo;
                equipmentInfo.id = equipmentVariadicItemInfo.id + (int)i;
                equipmentInfo.nickname = equipmentVariadicItemInfo.nickname;
                equipmentInfo.description = equipmentVariadicItemInfo.description;
                equipmentInfo.icon = equipmentVariadicItemInfo.icon;
                equipmentInfo.price = (uint)equipmentVariadicItemInfo.prices.GetPoint(i);
                equipmentInfo.variadicItemId = equipmentVariadicItemInfo.id;
                equipmentInfo.upgradeLevel = i;

                AssetDatabase.CreateAsset(itemInfo,
                    $"Assets/Resources/{ItemInfo.RESOURCE_PATH}/Equipment/{equipmentVariadicItemInfo.nickname}Upgrade{i}{ItemInfo.TYPE_NAME}.asset");
                AssetDatabase.SaveAssets();
            }
        }
#endif

    }


    public class VariadicItemInfoRegistry : EnumbaseClassRegistry<VariadicItemType, VariadicItemInfoBase>
    {
        public static VariadicItemInfoRegistry instance = new VariadicItemInfoRegistry();

        public override EnumbaseClassRegistry<VariadicItemType, VariadicItemInfoBase> Instance()
        {
            return instance;
        }

        public override VariadicItemInfoBase Create(VariadicItemType type)
        {
            switch (type)
            {
                case VariadicItemType.Equipment:
                    return new EquipmentVariadicItemInfo();
            }

            throw new Exception($"Invalid type: {type}");
        }
    }
}