using System;
using Data;
using Data.Static;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class QuestStackView : MonoBehaviour
    {
        public LayoutGroup questLayout;
        [NonSerialized] public Action onClick;

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh(Action onClick = null)
        {
            this.onClick = onClick;
            TransformUtil.DestroyChildren(questLayout.transform);

            var questPrefab = UIPrefabs.Instance.questStackSlotPrefab;
            
            foreach (var quest in QuestInventoryData.Instance.quests)
            {
                if (quest.rewardReceived) continue;
                var go = Instantiate(questPrefab, questLayout.transform, false);
                var questStackSlot = go.GetComponent<QuestStackSlot>();
                questStackSlot.SetData(quest, () => OnClick(quest, questStackSlot));
            }
            
            QuestInventoryData.Instance.onCountChanged = (questStack) => Refresh();
        }

        private void OnClick(QuestStack questStack, QuestStackSlot questStackSlot)
        {
            var mainTopBar = FindObjectOfType<MainTopBar>(true);
            mainTopBar.OnQuestClicked();
        }
    }
}