using Data.Static;
using UI.Components;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class UIPrefabs : SingletonScriptableObject<UIPrefabs>
    {
        public Button squareButtonPrefab;
        public OverheadGuagePanel overheadGuagePanelPrefab;
        public OverheadLootPanel overheadLootPanelPrefab;
        public ConfirmPopup confirmPopupPrefab;
        public MultipurposePopup multipurposePopup;
        public ItemSlot itemSlotPrefab;
        public ProductSlot productSlotPrefab;
        public StatChangeRow statChangeRowPrefab;
        public QuestSlot questSlotPrefab;
        public QuestStackSlot questStackSlotPrefab;


        protected override void OnInit()
        {
        }

        public override string GetResourcePath()
        {
            return "ScriptableObjects/Prefab/UIPrefabs";
        }

#if UNITY_EDITOR

        [MenuItem("Assets/Create/ScriptableObjects/Prefab/UIPrefabs")]
        public static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory("Assets/Resources/ScriptableObjects/Prefab");

            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<UIPrefabs>(),
                "Assets/Resources/ScriptableObjects/Prefab/UIPrefabs.asset");
        }
#endif
    }
}