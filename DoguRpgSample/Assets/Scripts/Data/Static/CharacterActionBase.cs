using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Static
{
    public enum CharacterActionType
    {
        Idle,
        Walk,
        Run,
        Jump,
        Attack,
        Hit,
        Die
    }

    [Serializable]
    public abstract class CharacterActionBase
    {
        public abstract CharacterActionType GetActionType();
        public AnimationClip animation;
    }

    [Serializable]
    public class CharacterActionIdle : CharacterActionBase
    {
        public override CharacterActionType GetActionType()
        {
            return CharacterActionType.Idle;
        }
    }

    [Serializable]
    public class CharacterActionWalk : CharacterActionBase
    {
        public override CharacterActionType GetActionType()
        {
            return CharacterActionType.Walk;
        }
    }

    [Serializable]
    public class CharacterActionRun : CharacterActionBase
    {
        public override CharacterActionType GetActionType()
        {
            return CharacterActionType.Run;
        }
    }

    [Serializable]
    public class CharacterActionJump : CharacterActionBase
    {
        public override CharacterActionType GetActionType()
        {
            return CharacterActionType.Jump;
        }
    }

    [Serializable]
    public class CharacterActionAttack : CharacterActionBase
    {
        public CharacterAttackCollider attackCollider;
        public float cooltime;

        public override CharacterActionType GetActionType()
        {
            return CharacterActionType.Attack;
        }
    }

    [Serializable]
    public class CharacterActionHit : CharacterActionBase
    {
        public override CharacterActionType GetActionType()
        {
            return CharacterActionType.Hit;
        }
    }

    [Serializable]
    public class CharacterActionDie : CharacterActionBase
    {
        public override CharacterActionType GetActionType()
        {
            return CharacterActionType.Die;
        }
    }

    public static class CharacterActionConverter
    {
        public static Dictionary<CharacterActionType, Func<CharacterActionBase>> constructors =
            new Dictionary<CharacterActionType, Func<CharacterActionBase>>()
            {
                { CharacterActionType.Idle, () => new CharacterActionIdle() { animation = null } },
                { CharacterActionType.Walk, () => new CharacterActionWalk() { animation = null } },
                { CharacterActionType.Run, () => new CharacterActionRun() { animation = null } },
                { CharacterActionType.Jump, () => new CharacterActionJump() { animation = null } },
                {
                    CharacterActionType.Attack,
                    () => new CharacterActionAttack() { animation = null, attackCollider = null }
                },
                { CharacterActionType.Hit, () => new CharacterActionHit() { animation = null } },
                { CharacterActionType.Die, () => new CharacterActionDie() { animation = null } }
            };
    }
}