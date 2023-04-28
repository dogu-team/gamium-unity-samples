using System;
using UI;
using UnityEngine;

public class OverheadGuagePanel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fillSpriteRenderer;
    [SerializeField] public Color color;
    [SerializeField] private float value;
    public float showDuration = 2.0f;
    private float lastUpdatedTime;

    public static OverheadGuagePanel Create(Color color, GameObject targetGo, float ratio)
    {
        var overheadGuagePrefab = UIPrefabs.Instance.overheadGuagePanelPrefab;
        var newGo = Instantiate(overheadGuagePrefab.gameObject, targetGo.transform, false);
        newGo.transform.localPosition = targetGo.transform.lossyScale.y * Vector3.up + Vector3.up * 0.2f;
        var panel = newGo.GetComponent<OverheadGuagePanel>();
        panel.color = color;
        panel.SetRatio(ratio);
        newGo.SetActive(false);
        return panel;
    }

    private void Start()
    {
        fillSpriteRenderer.color = color;
    }

    public void Update()
    {
        if (Time.realtimeSinceStartup - lastUpdatedTime > showDuration)
        {
            gameObject.SetActive(false);
        }

        var xPos = (1.0f - value) / 2.0f;
        fillSpriteRenderer.transform.localPosition = new Vector3(xPos, fillSpriteRenderer.transform.localPosition.y,
            fillSpriteRenderer.transform.localPosition.z);
        fillSpriteRenderer.transform.localScale = new Vector3(value, fillSpriteRenderer.transform.localScale.y,
            fillSpriteRenderer.transform.localScale.z);

        transform.LookAt(Camera.main.transform.position);
    }

    public void SetRatio(float value)
    {
        this.value = value;
        lastUpdatedTime = Time.realtimeSinceStartup;
        gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        fillSpriteRenderer.color = color;
        Update();
    }
}