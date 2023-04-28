using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Data.Static
{
    public class ShopInfo : ScriptableObject
    {
        public int id;
        public ItemStack[] productStacks;
        private const string RESOURCE_PATH = "ScriptableObjects/Data/Shop";
        private const string TYPE_NAME = "ShopInfo";


        private void OnValidate()
        {
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/" + RESOURCE_PATH + "/" + TYPE_NAME)]
        static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory($"Assets/Resources/{RESOURCE_PATH}");
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ShopInfo>(),
                $"Assets/Resources/{RESOURCE_PATH}/{TYPE_NAME}.asset");
        }
#endif
    }
}