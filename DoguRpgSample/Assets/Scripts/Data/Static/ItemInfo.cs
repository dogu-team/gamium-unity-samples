using UnityEditor;
using Util;

namespace Data.Static
{
    public class ItemInfo : EnumbaseScriptableObjectEntry<ItemType, ItemInfoBase>
    {
        public const string RESOURCE_PATH = "ScriptableObjects/Data/Item";
        public const string TYPE_NAME = "ItemInfo";

        public override ItemInfoBase Create(ItemType type)
        {
            return ItemInfoRegistry.instance.Create(type);
        }

        public string GetPrettyDescription()
        {
            var title = value.nickname;
            var body = value.description;
            var stats = "";
            var equipmentItemInfo = value as EquipmentItemInfo;
            if (null != equipmentItemInfo)
            {
                title += $" (Lv.{equipmentItemInfo.upgradeLevel})";
                var equipmentItemMetaInfo = equipmentItemInfo.GetVariadicInfo();
                stats += "\n";
                foreach (var statEntry in equipmentItemMetaInfo.stats)
                {
                    stats += $"{statEntry.type}: {statEntry.value.GetPoint(equipmentItemInfo.upgradeLevel)}\n";
                }
            }

            var ret = $"{title}\n" +
                      $"{body}\n" +
                      $"{stats}\n";

            return ret;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/" + RESOURCE_PATH + "/" + TYPE_NAME)]
        static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory($"Assets/Resources/{RESOURCE_PATH}");
            var newInfo = CreateInstance<ItemInfo>();
            newInfo.value = ItemInfoRegistry.instance.Create(ItemType.Coin);
            AssetDatabase.CreateAsset(newInfo,
                $"Assets/Resources/{RESOURCE_PATH}/{TYPE_NAME}.asset");
        }
#endif
    }
}