using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Static;
using UI.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class UISprites : SingletonScriptableObject<UISprites>
    {
        public SizeFixedEnumArray<EquipmentPosition, Sprite>.Entry[] equipmentSprites;

        public UISprites()
        {
            equipmentSprites = SizeFixedEnumArray<EquipmentPosition, Sprite>.Create();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!EditorConditions.IsValidatable()) return;
#endif
            SizeFixedEnumArray<EquipmentPosition, Sprite>.Validate(ref equipmentSprites);
        }

        protected override void OnInit()
        {
        }

        public override string GetResourcePath()
        {
            return "ScriptableObjects/Sprite/UISprites";
        }

#if UNITY_EDITOR

        [MenuItem("Assets/Create/ScriptableObjects/Sprite/UISprites")]
        public static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory("Assets/Resources/ScriptableObjects/Sprite");

            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<UISprites>(),
                "Assets/Resources/ScriptableObjects/Sprite/UISprites.asset");
        }
#endif
    }
}