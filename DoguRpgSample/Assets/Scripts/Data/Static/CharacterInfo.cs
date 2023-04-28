using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Util;

namespace Data.Static
{
    [Serializable]
    public class CharacterActionEntry
    {
        public CharacterActionType type;
        [SerializeReference] public CharacterActionBase action;
    }


    public class CharacterInfo : EnumbaseScriptableObjectEntry<CharacterType, CharacterInfoBase>
    {
        public List<CharacterActionEntry> actions = new List<CharacterActionEntry> { };
        public const string RESOURCE_PATH = "ScriptableObjects/Data/Character";
        public const string TYPE_NAME = "CharacterInfo";

        public override CharacterInfoBase Create(CharacterType type)
        {
            return CharacterInfoRegistry.instance.Create(type);
        }

        protected override void OnValidate()
        {
#if UNITY_EDITOR
            if (!EditorConditions.IsValidatable()) return;
#endif            
            base.OnValidate();

            value.stats.Validate();
            foreach (var stat in value.stats.stats)
            {
                ChartValue.ForceXMax(stat.value, LevelInfoList.Instance.expChart.GetPoints().Count);
            }


            ValidateAction();
        }

        private void ValidateAction()
        {
            if (actions.Count == Enum.GetValues(typeof(CharacterActionType)).Length)
            {
                return;
            }

            actions = new List<CharacterActionEntry>();
            foreach (var actionType in Enum.GetValues(typeof(CharacterActionType))
                         .Cast<CharacterActionType>())
            {
                actions.Add(new CharacterActionEntry
                {
                    type = actionType,
                    action = CharacterActionConverter.constructors[actionType]?.Invoke()
                });
            }
        }


#if UNITY_EDITOR
        [MenuItem("Assets/Create/" + RESOURCE_PATH + "/" + TYPE_NAME)]
        static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory($"Assets/Resources/{RESOURCE_PATH}");
            var newInfo = CreateInstance<CharacterInfo>();
            newInfo.value = CharacterInfoRegistry.instance.Create(CharacterType.Player);
            AssetDatabase.CreateAsset(newInfo, $"Assets/Resources/{RESOURCE_PATH}/{TYPE_NAME}.asset");
        }
#endif
    }
}