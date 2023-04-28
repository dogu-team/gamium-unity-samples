using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Data;
using Data.Static;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(CharacterInfoController))]
[RequireComponent(typeof(CharacterAnimator))]
public class CharacterActionController : MonoBehaviour
{
    public bool isAttacking { get; private set; }
    public bool isGettingHit { get; private set; }
    public bool isDead { get; private set; }
    public Action onDead;


    public CharacterStatController stat { get; private set; }
    private CharacterInfoController infoController;
    private CharacterAnimator characterAnimator;

    private float lastAttackTime = 0;

    private void Awake()
    {
        infoController = GetComponent<CharacterInfoController>();
        characterAnimator = GetComponent<CharacterAnimator>();
        stat = GetComponent<CharacterStatController>();
        if (!infoController.info.actions.Any())
        {
            throw new Exception("No action infos found");
        }
    }

    public void Idle()
    {
        var action = GetAction(CharacterActionType.Idle);
        characterAnimator.PlayOnlyIfNotPlaying(action.animation.name);
    }

    public void Walk()
    {
        var action = GetAction(CharacterActionType.Walk);
        characterAnimator.PlayOnlyIfNotPlaying(action.animation.name);
    }

    public void Run()
    {
        var action = GetAction(CharacterActionType.Run);
        characterAnimator.PlayOnlyIfNotPlaying(action.animation.name);
    }

    public void Attack()
    {
        var action = GetAction(CharacterActionType.Attack) as CharacterActionAttack;
        if (Time.realtimeSinceStartup - lastAttackTime < action.cooltime)
        {
            return;
        }

        var colliderGo = Instantiate(action.attackCollider.gameObject, transform, false);
        var collider = colliderGo.GetComponent<CharacterAttackCollider>();
        var attackedTargets = new HashSet<GameObject>();
        collider.onTriggerEnter = OnAttackCollide(attackedTargets, collider);
        isAttacking = true;
        lastAttackTime = Time.realtimeSinceStartup;
        characterAnimator.PlayOnce(action.animation.name, () =>
        {
            colliderGo.SetActive(false);
            Destroy(colliderGo);
            isAttacking = false;
            if (!characterAnimator.HasWait)
            {
                Idle();
            }
        });
    }

    private Action<Collider> OnAttackCollide(HashSet<GameObject> attackedTargets, CharacterAttackCollider collider)
    {
        return (other) =>
        {
            if (attackedTargets.Contains(other.gameObject))
            {
                return;
            }

            if (!other.gameObject.CompareTag(collider.targetType.ToTag()))
            {
                return;
            }

            var characterActionController = other.gameObject.GetComponent<CharacterActionController>();
            if (null == characterActionController)
            {
                return;
            }

            characterActionController.GetHit(stat);
            attackedTargets.Add(other.gameObject);
        };
    }

    public void GetHit(CharacterStatController fromStatController)
    {
        var action = GetAction(CharacterActionType.Hit);
        var damage =
            Math.Clamp(fromStatController.Get(Stat.StatType.Attack).Value - stat.Get(Stat.StatType.Defense).Value, 0,
                Int64.MaxValue);
        stat.TakeDamage(damage);
        if (stat.Get(Stat.StatType.Health).Value <= 0)
        {
            Die(null);
            return;
        }

        isGettingHit = true;
        characterAnimator.PlayOnce(action.animation.name, () =>
        {
            isGettingHit = false;
            if (!characterAnimator.HasWait)
            {
                Idle();
            }
        });
    }

    public void Die(Action completeListener)
    {
        if (isDead)
        {
            return;
        }

        var action = GetAction(CharacterActionType.Die);
        isDead = true;
        characterAnimator.PlayOnce(action.animation.name, null);
        onDead?.Invoke();
        completeListener?.Invoke();
    }


    public CharacterActionBase GetAction(CharacterActionType type)
    {
        var action = infoController.actionsIndex[type];
        return action;
    }
}