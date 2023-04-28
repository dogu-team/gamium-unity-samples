using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}