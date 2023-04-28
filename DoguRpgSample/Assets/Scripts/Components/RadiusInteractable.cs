using System;
using Data.Static;
using TMPro;
using UI;
using UI.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class RadiusInteractable : MonoBehaviour
{
    public float interactRadius = 1.0f;
    public TextMesh text;
    protected bool isInteracting;
    private float detachedTime;
    private float attachedTime;
    private GameObject player;

    protected abstract void OnInteractStart();
    protected abstract void OnInteractEnd();

    protected virtual void Start()
    {
        var playerCharacterController = FindObjectOfType<PlayerCharacterController>();
        player = playerCharacterController.gameObject;
    }

    private void Update()
    {
        float targetDistance = Vector3.Distance(player.transform.position, transform.position);
        if (targetDistance <= interactRadius)
        {
            if (attachedTime < detachedTime)
            {
                if (!isInteracting)
                {
                    OnInteractStart();
                }

                isInteracting = true;
            }

            attachedTime = Time.realtimeSinceStartup;
        }
        else
        {
            if (isInteracting)
            {
                isInteracting = false;
                OnInteractEnd();
            }

            detachedTime = Time.realtimeSinceStartup;
        }

        if (null != text)
        {
            text.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }


    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}