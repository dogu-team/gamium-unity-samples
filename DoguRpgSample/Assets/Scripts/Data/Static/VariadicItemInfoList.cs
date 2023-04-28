using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Util;

namespace Data.Static
{
    public class VariadicItemInfoList : EnumbaseScriptableObjectList<VariadicItemInfoList, VariadicItemType,
        VariadicItemInfoBase, VariadicItemInfo>
    {
        private const string RESOURCE_DIRECTORY_PATH = "ScriptableObjects/Data/Item";
        private const string TYPE_NAME = "VariadicItemInfoList";

        public override string GetResourceDirectoryPath() => RESOURCE_DIRECTORY_PATH;
        public override string GetTypeName() => TYPE_NAME;


#if UNITY_EDITOR
        [MenuItem("Assets/Create/" + RESOURCE_DIRECTORY_PATH + "/" + TYPE_NAME)]
        static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory($"Assets/Resources/{RESOURCE_DIRECTORY_PATH}");
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<VariadicItemInfoList>(),
                $"Assets/Resources/{RESOURCE_DIRECTORY_PATH}/{TYPE_NAME}.asset");
        }
#endif
    }
}