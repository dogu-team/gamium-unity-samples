using System;
using Data.Static;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class QuestStack
    {
        public int questId;
        public uint count;
        public bool rewardReceived;
        
        public void Increment(uint argCount)
        {
            ulong sum = (ulong)Mathf.Clamp((ulong)count + (ulong)argCount, uint.MinValue, uint.MaxValue);
            count = (uint)sum;
        }

        public bool IsCompleted()
        {
            var questInfo = GetQuestInfo();
            return questInfo.value.IsCompleted(this);
        }

        public QuestInfo GetQuestInfo()
        {
            return QuestInfoList.Instance.GetEntry(questId);
        }
    }
}