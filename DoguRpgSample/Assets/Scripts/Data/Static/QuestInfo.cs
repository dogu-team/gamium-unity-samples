using UnityEditor;
using UnityEngine;
using Util;

namespace Data.Static
{
    public class QuestInfo : EnumbaseScriptableObjectEntry<QuestType, QuestInfoBase>
    {
        public const string RESOURCE_PATH = "ScriptableObjects/Data/Quest";
        public const string TYPE_NAME = "QuestInfo";

        public override QuestInfoBase Create(QuestType type)
        {
            return QuestInfoRegistry.instance.Create(type);
        }


#if UNITY_EDITOR
        [MenuItem("Assets/Create/" + RESOURCE_PATH + "/" + TYPE_NAME)]
        static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory($"Assets/Resources/{RESOURCE_PATH}");
            var newInfo = CreateInstance<QuestInfo>();
            newInfo.value = QuestInfoRegistry.instance.Create(QuestType.KillEnemy);
            AssetDatabase.CreateAsset(newInfo,
                $"Assets/Resources/{RESOURCE_PATH}/{TYPE_NAME}.asset");
        }
#endif
    }
}