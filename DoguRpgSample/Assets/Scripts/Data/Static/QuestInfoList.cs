using UnityEditor;
using Util;

namespace Data.Static
{
    public class QuestInfoList : EnumbaseScriptableObjectList<QuestInfoList, QuestType, QuestInfoBase, QuestInfo>
    {
        private const string RESOURCE_DIRECTORY_PATH = "ScriptableObjects/Data/Quest";
        private const string TYPE_NAME = "QuestInfoList";

        public override string GetResourceDirectoryPath() => RESOURCE_DIRECTORY_PATH;
        public override string GetTypeName() => TYPE_NAME;

#if UNITY_EDITOR
        [MenuItem("Assets/Create/" + RESOURCE_DIRECTORY_PATH + "/" + TYPE_NAME)]
        static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory($"Assets/Resources/{RESOURCE_DIRECTORY_PATH}");
            AssetDatabase.CreateAsset(CreateInstance<QuestInfoList>(),
                $"Assets/Resources/{RESOURCE_DIRECTORY_PATH}/{TYPE_NAME}.asset");
        }
#endif
    }
}