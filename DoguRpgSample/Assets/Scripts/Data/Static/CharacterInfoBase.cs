using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Static
{
    [Serializable]
    public abstract class CharacterInfoBase : EnumbaseClass<CharacterType>
    {
        public GameObject prefab;
        public string nickname;
        [TextArea] public string description;
        public StaticStats stats = new StaticStats();
    }

    [Serializable]
    public class PlayerCharacterInfo : CharacterInfoBase
    {
        public override CharacterType GetDataType() => CharacterType.Player;
    }

    [Serializable]
    public class EnemyCharacterInfo : CharacterInfoBase
    {
        public override CharacterType GetDataType() => CharacterType.Enemy;
        public float lookRadius = 10.0f;
        public long exp;
        public LootInfo[] loots;
    }

    public class CharacterInfoRegistry : EnumbaseClassRegistry<CharacterType, CharacterInfoBase>
    {
        public static CharacterInfoRegistry instance = new CharacterInfoRegistry();

        public override EnumbaseClassRegistry<CharacterType, CharacterInfoBase> Instance()
        {
            return instance;
        }

        public override CharacterInfoBase Create(CharacterType type)
        {
            switch (type)
            {
                case CharacterType.Player:
                    return new PlayerCharacterInfo();
                case CharacterType.Enemy:
                    return new EnemyCharacterInfo();
            }

            throw new Exception($"Invalid type: {type}");
        }
    }
}