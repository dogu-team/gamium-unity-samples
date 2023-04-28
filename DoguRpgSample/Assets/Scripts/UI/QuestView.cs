using System;
using Data;
using Data.Static;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class QuestView : MonoBehaviour
    {
        public LayoutGroup questLayout;
        [NonSerialized] public Action onClosed;
        [NonSerialized] public Action<QuestInfo> onClick;

        public void Refresh(Action<QuestInfo> onClick = null)
        {
            this.onClick = onClick;
            TransformUtil.DestroyChildren(questLayout.transform);

            var questPrefab = UIPrefabs.Instance.questSlotPrefab;
            
            foreach (var quest in QuestInfoList.Instance.entries)
            {
                var go = Instantiate(questPrefab, questLayout.transform, false);
                var questSlot = go.GetComponent<QuestSlot>();
                questSlot.SetData(quest, () => OnClick(quest, questSlot));
            }
        }
        
        public void OnClose()
        {
            gameObject.SetActive(false);
            onClosed?.Invoke();
        }

        private void OnClick(QuestInfo questInfo, QuestSlot questSlot)
        {
            var questStack = QuestInventoryData.Instance.GetData(questInfo);
            if (null == questStack)
            {
                QuestInventoryData.Instance.IncrementCount(questInfo, 0);
                questSlot.Refresh();
                return;
            }

            if (questStack.IsCompleted())
            {
                InventoryData.Instance.IncrementItem(questInfo.value.rewardItem, questInfo.value.rewardAmount);
                questStack.rewardReceived = true;
            }
            questSlot.Refresh();
        }
    }
}