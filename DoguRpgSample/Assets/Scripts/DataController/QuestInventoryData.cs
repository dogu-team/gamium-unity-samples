using System;
using System.Collections.Generic;
using System.Linq;
using Data.Static;
using Util;

namespace Data
{
    public class QuestInventoryData : SingletonMonoBehavior<QuestInventoryData>
    {
        private const string QUEST_INVENTORY_DATA = "QuestInventoryData";
        public List<QuestStack> quests;
        public Action<QuestStack> onCountChanged;
        
        protected void Awake()
        {
            quests = JsonPrefs.Load<List<QuestStack>>(QUEST_INVENTORY_DATA);
            if (null == quests)
            {
                quests = new List<QuestStack>();
            }
        }
        
        public QuestStack GetData(QuestInfo questInfo)
        {
            return quests.FirstOrDefault(item => item.questId == questInfo.value.id);
        }
        
        public void IncrementCount(QuestInfo questInfo, uint count)
        {
            IncrementCountInternal(questInfo, count);
            Save();
        }
        
        public void OnEnemyDead(CharacterInfo characterInfo)
        {
            var targetQuests = quests.Where(q =>
            {
                var questInfo = q.GetQuestInfo();
                if (questInfo.type != QuestType.KillEnemy) return false;
                var killEnemyQuestInfo = questInfo.value as KillEnemyQuestInfo;
                if (killEnemyQuestInfo.target.value.id != characterInfo.value.id) return false;
                return true;
            });
            foreach (var quest in targetQuests)
            {
                IncrementCountInternal(quest.GetQuestInfo(), 1);
            }
            Save();
        }

        
        private void IncrementCountInternal(QuestInfo questInfo, uint count)
        {
            var questStack = quests.Find(i => i.questId == questInfo.value.id);
            if (null == questStack)
            {
                questStack = new QuestStack
                {
                    questId = questInfo.value.id,
                    count = 0,
                };
                quests.Add(questStack);
            }

            if (questStack.IsCompleted())
            {
                return;
            }
            
            questStack.Increment(count);
            onCountChanged?.Invoke(questStack);

        }

        
        private void Save()
        {
            JsonPrefs.Save(QUEST_INVENTORY_DATA, quests);
        }
    }
}