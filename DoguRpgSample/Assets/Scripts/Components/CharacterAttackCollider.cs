using System;
using Data;
using UnityEngine;

public class CharacterAttackCollider : MonoBehaviour
{
    public CharacterType targetType;
    public Action<Collider> onTriggerEnter;
    public Action<Collider> onTriggerStay;
    public Action<Collider> onTriggerExit;


    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(other);
    }
}