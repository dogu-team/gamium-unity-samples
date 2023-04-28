using System;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public Data.Static.CharacterInfo enemyInfo;

    void OnDrawGizmosSelected()
    {
        var enemyCharacterInfo = enemyInfo.value as Data.Static.EnemyCharacterInfo;

        Gizmos.color = Color.blue;
        Gizmos.matrix = transform.localToWorldMatrix * Matrix4x4.Rotate(Quaternion.Euler(-90, 0, 0));
        var skinMeshRenderer = enemyCharacterInfo.prefab.GetComponentInChildren<SkinnedMeshRenderer>();
        Gizmos.DrawWireMesh(skinMeshRenderer.sharedMesh, Vector3.zero);
    }
}