using System;
using Data;
using Data.Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class QuestSlot : MonoBehaviour
    {
        public Text title;
        public Text description;
        public Button button;
        public Text buttonText;
        [NonSerialized] public Action onClick;
        [NonSerialized] public QuestInfo questInfo;

        public void SetData(QuestInfo questInfo, Action onClick)
        {
            this.questInfo = questInfo;
            this.onClick = onClick;
            Refresh();
        }


        public void Refresh()
        {
            title.text = questInfo.value.nickname;
            description.text = questInfo.value.GetPrettyDescripton();
            
            buttonText.text = "Accept";
            button.enabled = true;

            var questStack = QuestInventoryData.Instance.GetData(questInfo);
            if (null != questStack)
            {
                if (questStack.IsCompleted() && !questStack.rewardReceived)
                {
                    buttonText.text = "Get Reward";
                    button.enabled = true;
                }
                else if (questStack.IsCompleted() && questStack.rewardReceived)
                {
                    buttonText.text = "Reward\nReceived";
                    button.enabled = false;
                }
                else
                {
                    buttonText.text = "Progressing";
                    button.enabled = false;
                }
            }
        }

        public void OnClick()
        {
            onClick?.Invoke();
        }
    }
}