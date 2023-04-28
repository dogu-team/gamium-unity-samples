using System;
using Data;
using Data.Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class QuestStackSlot : MonoBehaviour
    {
        public Text title;
        public Text description;
        [NonSerialized] public Action onClick;
        [NonSerialized] public QuestStack questStack;

        public void SetData(QuestStack questStack, Action onClick)
        {
            this.questStack = questStack;
            this.onClick = onClick;
            Refresh();
        }

        public void Refresh()
        {
            var questInfo = questStack.GetQuestInfo();
            title.text = questInfo.value.nickname;
            description.text = questInfo.value.GetProgressDescription(questStack);
        }

        public void OnClick()
        {
            onClick?.Invoke();
        }
    }
}