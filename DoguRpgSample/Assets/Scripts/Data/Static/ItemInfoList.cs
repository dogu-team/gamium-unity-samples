using System;
using System.Linq;
using UnityEditor;
using Util;

namespace Data.Static
{
    public class ItemInfoList : EnumbaseScriptableObjectList<ItemInfoList, ItemType, ItemInfoBase, ItemInfo>
    {
        private const string RESOURCE_DIRECTORY_PATH = "ScriptableObjects/Data/Item";
        private const string TYPE_NAME = "ItemInfoList";

        public override string GetResourceDirectoryPath() => RESOURCE_DIRECTORY_PATH;
        public override string GetTypeName() => TYPE_NAME;

        public ItemInfo GetCoin()
        {
            var c = entries.First(i => i.type == ItemType.Coin);
            if (null == c)
            {
                throw new Exception("No coin found");
            }

            return c;
        }


#if UNITY_EDITOR
        [MenuItem("Assets/Create/" + RESOURCE_DIRECTORY_PATH + "/" + TYPE_NAME)]
        static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory($"Assets/Resources/{RESOURCE_DIRECTORY_PATH}");
            AssetDatabase.CreateAsset(CreateInstance<ItemInfoList>(),
                $"Assets/Resources/{RESOURCE_DIRECTORY_PATH}/{TYPE_NAME}.asset");
        }
#endif
    }
}