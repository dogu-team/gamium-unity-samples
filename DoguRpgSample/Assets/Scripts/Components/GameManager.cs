using System;
using Data;
using Data.Static;
using MobileInput;
using UI;
using UI.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Util;
using CharacterInfo = Data.Static.CharacterInfo;

public class GameManager : MonoBehaviour
{
    // ui
    public MobileInputController mobileInputController;
    public Transform playerParent;
    public Transform enemiesParent;
    public Transform enemySpawnPointsParent;
    public Color enemyOverheadGuageColor;
    [field: NonSerialized] public PlayerCharacterController playerCharacterController { get; private set; }

    private void Awake()
    {
        SpawnPlayer();
        SpawnEnemies();
    }

    private void SpawnPlayer()
    {
        var playerData = PlayerDataController.Instance.GetCurrentPlayerCharacter();
        var characterInfo = CharacterInfoList.Instance.GetEntry(playerData.characterId);
        var newPlayerGo = Instantiate(characterInfo.value.prefab, playerParent.transform, false);
        newPlayerGo.SetActive(false);
        playerCharacterController = newPlayerGo.AddComponent<PlayerCharacterController>();
        playerCharacterController.mobileInputController = mobileInputController;
        newPlayerGo.SetActive(true);
    }

    private void SpawnEnemies()
    {
        var spawnPoints = enemySpawnPointsParent.GetComponentsInChildren<EnemySpawnPoint>();
        foreach (var spawnPoint in spawnPoints)
        {
            SpawnEnemy(spawnPoint);
        }
    }

    private void SpawnEnemy(EnemySpawnPoint spawnPoint)
    {
        var spawnPointTransform = spawnPoint.transform;
        var enemyCharacterInfo = spawnPoint.enemyInfo;
        var enemyInfo = spawnPoint.enemyInfo.value as EnemyCharacterInfo;

        var newEnemyGo = Instantiate(enemyCharacterInfo.value.prefab, enemiesParent.transform, false);

        newEnemyGo.SetActive(false);
        var position = spawnPointTransform.position;
        newEnemyGo.transform.position = position;
        newEnemyGo.transform.rotation = spawnPointTransform.rotation;

        var enemyController = newEnemyGo.AddComponent<EnemyController>();
        enemyController.player = playerCharacterController;
        enemyController.enemyInfo = enemyCharacterInfo;
        enemyController.spawnPoint = position;
        enemyController.onDead = () => OnEnemyDead(spawnPoint.enemyInfo, newEnemyGo, spawnPoint);
        newEnemyGo.SetActive(true);

        var overheadGuage = OverheadGuagePanel.Create(enemyOverheadGuageColor, newEnemyGo,
            1.0f);
        enemyController.actionController.stat.Get(Stat.StatType.Health).onValueChanged =
            (health) => OnEnemyHealthChanged(overheadGuage, enemyCharacterInfo, health.Value);
    }

    private void OnEnemyHealthChanged(OverheadGuagePanel guagePanel, CharacterInfo enemyInfo, long health)
    {
        guagePanel.SetRatio((float)health / (float)enemyInfo.value.stats.Get(Stat.StatType.Health).GetPoint(0));
    }

    private void OnEnemyDead(CharacterInfo characterInfo, GameObject enemyGo, EnemySpawnPoint spawnPoint)
    {
        var enemyInfo = characterInfo.value as EnemyCharacterInfo;

        var playerExp = playerCharacterController.actionController.stat.Get(Stat.StatType.Experience);
        playerExp.Increment(enemyInfo.exp);
        
        QuestInventoryData.Instance.OnEnemyDead(characterInfo);
        
        foreach (var loot in enemyInfo.loots)
        {
            var isGet = UnityEngine.Random.value < loot.chance;
            if (!isGet)
            {
                continue;
            }

            OverheadLootPanel.Create(loot, enemyGo, 1.5f);
            InventoryData.Instance.IncrementItem(loot.item, loot.amount);
        }


        TaskManager.RunNextTime(() =>
        {
            Destroy(enemyGo);
            TaskManager.RunNextTime(
                () => { SpawnEnemy(spawnPoint); }, 10.0f);
        }, 3.0f);
    }
}