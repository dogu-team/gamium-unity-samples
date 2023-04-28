using System;
using System.Collections;
using System.Threading.Tasks;
using MobileInput;
using UnityEngine;
using UnityEngine.AI;

public class CameraController : MonoBehaviour
{
    public NavMeshAgent target;
    public Vector3 closeOffset;
    public Vector3 farOffset;
    public float zoomSpeed = 0.1f;
    public float pitch = 1.3f;
    public float yawSpeed = 200f;
    public float yOffsetSpeed = 10f;
    public MobileInputController mobileInputController;
    private float currentZoom = 0.8f;
    private float currentYaw = 0f;
    private float currentYOffset = 0.0f;
    private bool hasAnimationTask;


    void Update()
    {
        if (hasAnimationTask)
        {
            return;
        }

        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, 0.0f, 1.0f);

        currentYaw -= mobileInputController.Yaw * yawSpeed * Time.deltaTime;
        currentYOffset -= mobileInputController.Pitch * yOffsetSpeed * Time.deltaTime;
        currentYOffset = Mathf.Clamp(currentYOffset, -0.5f, 1.0f);
    }

    private void LateUpdate()
    {
        var offset = Vector3.Lerp(closeOffset, farOffset, currentZoom);
        offset.y += currentYOffset;

        var position = target.transform.position;
        transform.position = position - offset;
        transform.LookAt(position + Vector3.up * pitch);
        transform.RotateAround(position, Vector3.up, currentYaw);
    }


    public void AnimateZoom(float zoomValue)
    {
        if (hasAnimationTask)
        {
            return;
        }

        StartCoroutine(CoAnimateZoom(zoomValue));
    }

    private IEnumerator CoAnimateZoom(float zoomValue)
    {
        zoomValue = Mathf.Clamp(zoomValue, 0.0f, 1.0f);

        hasAnimationTask = true;
        var startZoom = currentZoom;
        var startTime = Time.time;
        var duration = 1.0f;
        while (Time.time < startTime + duration)
        {
            currentZoom = Mathf.Lerp(startZoom, zoomValue, (Time.time - startTime) / duration);
            yield return null;
        }

        currentZoom = zoomValue;
        hasAnimationTask = false;
    }
}