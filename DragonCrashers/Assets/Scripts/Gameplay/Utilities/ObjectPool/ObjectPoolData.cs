using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_ObjectPool_", menuName = "Utilities/Object Pool", order = 1)]
public class ObjectPoolData : ScriptableObject
{
    [Header("Settings")]
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand;
}
