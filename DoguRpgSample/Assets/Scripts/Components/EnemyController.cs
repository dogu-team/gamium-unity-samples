using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Data.Static.CharacterInfo enemyInfo;
    public Vector3 spawnPoint;
    public PlayerCharacterController player;
    public CharacterActionController actionController { get; private set; }
    public Action onDead;
    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        actionController = GetComponent<CharacterActionController>();
        actionController.onDead = () =>
        {
            agent.isStopped = true;
            onDead?.Invoke();
        };

        var stat = actionController.stat;
        stat.ApplyCharacterInfo(enemyInfo, 0);
    }

    void Update()
    {
        float distanceFromSpawn = Vector3.Distance(spawnPoint, transform.position);
        float targetDistance = Vector3.Distance(player.transform.position, transform.position);
        UpdateMove(distanceFromSpawn, targetDistance);

        if (targetDistance <= agent.stoppingDistance)
        {
            FaceTarget();
            DoAttack();
        }
    }

    void UpdateMove(float distanceFromSpawn, float targetDistance)
    {
        var enemyCharacterInfo = enemyInfo.value as Data.Static.EnemyCharacterInfo;
        if (actionController.isGettingHit || actionController.isAttacking || actionController.isDead)
        {
            return;
        }

        if (targetDistance <= enemyCharacterInfo.lookRadius)
        {
            agent.SetDestination(player.transform.position);
            return;
        }

        if (distanceFromSpawn >= enemyCharacterInfo.lookRadius)
        {
            actionController.Walk();
            agent.SetDestination(spawnPoint);
            return;
        }

        actionController.Idle();
    }

    void DoAttack()
    {
        if (actionController.isGettingHit || actionController.isAttacking || actionController.isDead)
        {
            return;
        }

        actionController.Attack();
    }

    void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        var enemyCharacterInfo = enemyInfo.value as Data.Static.EnemyCharacterInfo;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyCharacterInfo.lookRadius);
    }
}