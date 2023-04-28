using System;
using UnityEngine;

namespace Data.Static
{
    [Serializable]
    public abstract class QuestInfoBase : EnumbaseClass<QuestType>
    {
        public string nickname;
        [TextArea] public string description;
        public ItemInfo rewardItem;
        public uint rewardAmount;

        public virtual string GetPrettyDescripton()
        {
            return description;
        }

        public abstract bool IsCompleted(QuestStack stack);
        public virtual string GetProgressDescription(QuestStack stack)
        {
            return stack.count.ToString();
        }
    }

    [Serializable]
    public class KillEnemyQuestInfo : QuestInfoBase
    {
        public override QuestType GetDataType() => QuestType.KillEnemy;
        public override string GetPrettyDescripton()
        {
            return $"{description}\n Kill {target.value.nickname} {amount} times. Reward: {rewardItem.value.nickname} {rewardAmount}";
        }

        public override bool IsCompleted(QuestStack stack)
        {
            return stack.count >= amount;
        }
        
        public override string GetProgressDescription(QuestStack stack)
        {
            return $"{stack.count} / {amount}";
        }


        public CharacterInfo target;
        public uint amount;
    }

    [Serializable]
    public class CollectItemQuestInfo : QuestInfoBase
    {
        public override QuestType GetDataType() => QuestType.CollectItem;
        public override bool IsCompleted(QuestStack stack)
        {
            return false;
        }
    }


    public class QuestInfoRegistry : EnumbaseClassRegistry<QuestType, QuestInfoBase>
    {
        public static QuestInfoRegistry instance = new QuestInfoRegistry();

        public override EnumbaseClassRegistry<QuestType, QuestInfoBase> Instance()
        {
            return instance;
        }

        public override QuestInfoBase Create(QuestType type)
        {
            switch (type)
            {
                case QuestType.KillEnemy:
                    return new KillEnemyQuestInfo();
                case QuestType.CollectItem:
                    return new CollectItemQuestInfo();
            }

            throw new Exception($"Invalid type: {type}");
        }
    }
}