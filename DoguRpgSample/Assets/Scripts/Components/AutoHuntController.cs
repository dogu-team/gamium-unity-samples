using System.Collections.Generic;
using Data.Static;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Util;

namespace Components
{
    public class AutoHuntController : SingletonMonoBehavior<AutoHuntController>
    {
        private class AttackableTarget
        {
            public int frame;
            public EnemyController enemy;
        }

        private bool isAutoHunt = false;
        private PlayerCharacterController playerCharacterController;
        private List<AttackableTarget> attackableTargets = new List<AttackableTarget>();
        private int attackableTargetValidFrame = 5;

        public void SetAutoHunt(bool isAutoHunt)
        {
            this.isAutoHunt = isAutoHunt;
            if (!this.isAutoHunt && null != playerCharacterController)
            {
                var navMeshAgent = playerCharacterController.GetComponent<NavMeshAgent>();
                navMeshAgent.ResetPath();
                playerCharacterController.target = null;
            }

            if (this.isAutoHunt)
            {
                var cameraController = Camera.main.GetComponent<CameraController>();
                cameraController.AnimateZoom(1.0f);
            }
        }


        private void Update()
        {
            if (!isAutoHunt)
            {
                return;
            }

            CheckPlayerCharacterController();
            CheckAttackableTargets();
            var navMeshAgent = playerCharacterController.GetComponent<NavMeshAgent>();
            if (navMeshAgent == null)
            {
                return;
            }

            if (playerCharacterController.actionController.isAttacking ||
                playerCharacterController.actionController.isGettingHit)
            {
                return;
            }

            if (attackableTargets.Count > 0)
            {
                navMeshAgent.ResetPath();
                playerCharacterController.target = attackableTargets[0].enemy;
                playerCharacterController.actionController.Attack();
                return;
            }

            if (!GetNearestEnemy(out var enemy, out var distance))
            {
                return;
            }

            navMeshAgent.SetDestination(enemy.transform.position);
        }

        private void CheckPlayerCharacterController()
        {
            if (null != playerCharacterController)
            {
                return;
            }

            playerCharacterController = FindObjectOfType<PlayerCharacterController>();
            var attackAction =
                playerCharacterController.actionController.GetAction(CharacterActionType.Attack) as
                    CharacterActionAttack;
            var colliderGo = Instantiate(attackAction.attackCollider.gameObject, playerCharacterController.transform,
                false);
            colliderGo.name = "AutoHuntPredictionAttackCollider";
            var collider = colliderGo.GetComponent<CharacterAttackCollider>();
            collider.onTriggerStay = (other) =>
            {
                var enemy = other.gameObject.GetComponent<EnemyController>();
                if (null == enemy || CheckInvalidEnemy(enemy))
                {
                    return;
                }

                var attackableTarget = attackableTargets.Find(x => x.enemy == enemy);
                if (null == attackableTarget)
                {
                    attackableTargets.Add(new AttackableTarget()
                    {
                        frame = attackableTargetValidFrame,
                        enemy = enemy
                    });
                }
                else
                {
                    attackableTarget.frame = attackableTargetValidFrame;
                }
            };
        }

        private void CheckAttackableTargets()
        {
            for (var i = attackableTargets.Count - 1; i >= 0; i--)
            {
                var attackableTarget = attackableTargets[i];
                if (CheckInvalidEnemy(attackableTarget.enemy))
                {
                    attackableTargets.RemoveAt(i);
                    return;
                }

                attackableTarget.frame--;
                if (attackableTarget.frame <= 0)
                {
                    attackableTargets.RemoveAt(i);
                }
            }
        }

        private bool GetNearestEnemy(out EnemyController enemy, out float distance)
        {
            enemy = null;
            distance = float.MaxValue;
            var enemies = FindObjectsOfType<EnemyController>();
            foreach (var e in enemies)
            {
                if (CheckInvalidEnemy(e))
                {
                    continue;
                }

                var d = Vector3.Distance(e.transform.position, playerCharacterController.transform.position);
                if (d < distance)
                {
                    distance = d;
                    enemy = e;
                }
            }

            return null != enemy;
        }

        private bool CheckInvalidEnemy(EnemyController enemy)
        {
            if (enemy.actionController.isDead)
            {
                return true;
            }

            return false;
        }
    }
}