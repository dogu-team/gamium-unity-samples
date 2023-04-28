using System.Linq;
using UnityEditor;
using UnityEngine;
using Util;

namespace Data.Static
{
    public class CharacterInfoList : EnumbaseScriptableObjectList<CharacterInfoList, CharacterType, CharacterInfoBase,
        CharacterInfo>
    {
        private const string RESOURCE_DIRECTORY_PATH = "ScriptableObjects/Data/Character";
        private const string TYPE_NAME = "CharacterInfoList";

        public override string GetResourceDirectoryPath() => RESOURCE_DIRECTORY_PATH;
        public override string GetTypeName() => TYPE_NAME;


#if UNITY_EDITOR
        [MenuItem("Assets/Create/" + RESOURCE_DIRECTORY_PATH + "/" + TYPE_NAME)]
        static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory($"Assets/Resources/{RESOURCE_DIRECTORY_PATH}");
            AssetDatabase.CreateAsset(CreateInstance<CharacterInfoList>(),
                $"Assets/Resources/{RESOURCE_DIRECTORY_PATH}/{TYPE_NAME}.asset");
        }
#endif
    }
}