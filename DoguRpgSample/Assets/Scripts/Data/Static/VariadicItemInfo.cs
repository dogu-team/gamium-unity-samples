using UnityEditor;
using UnityEngine;
using Util;

namespace Data.Static
{
    public class VariadicItemInfo : EnumbaseScriptableObjectEntry<VariadicItemType, VariadicItemInfoBase>
    {
        public const string RESOURCE_PATH = "ScriptableObjects/Data/Item";
        public const string TYPE_NAME = "VariadicItemInfo";

        protected override void OnValidate()
        {
#if UNITY_EDITOR
            if (!EditorConditions.IsValidatable()) return;
#endif
            base.OnValidate();
            this.value.OnValidate();
        }

        public override VariadicItemInfoBase Create(VariadicItemType type)
        {
            return VariadicItemInfoRegistry.instance.Create(type);
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/" + RESOURCE_PATH + "/" + TYPE_NAME)]
        public static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory($"Assets/Resources/{RESOURCE_PATH}");
            var newInfo = CreateInstance<VariadicItemInfo>();
            newInfo.value = VariadicItemInfoRegistry.instance.Create(VariadicItemType.Equipment);
            AssetDatabase.CreateAsset(newInfo,
                $"Assets/Resources/{RESOURCE_PATH}/{TYPE_NAME}.asset");
        }
#endif
    }
}