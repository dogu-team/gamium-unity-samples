using System;
using Data.Static;
using UI;
using UnityEngine;

public class OverheadLootPanel : MonoBehaviour
{
    [NonSerialized] public LootInfo lootInfo;
    [NonSerialized] public float duration;
    public SpriteRenderer spriteRenderer;
    public TextMesh text;
    private float createdTime;

    public static OverheadLootPanel Create(LootInfo lootInfo, GameObject targetGo, float duration)
    {
        var overheadLootPanelPrefab = UIPrefabs.Instance.overheadLootPanelPrefab;
        var newGo = Instantiate(overheadLootPanelPrefab.gameObject);
        newGo.transform.position = targetGo.transform.position + targetGo.transform.lossyScale.y * Vector3.up +
                                   Vector3.up * 0.2f;
        var panel = newGo.GetComponent<OverheadLootPanel>();
        panel.lootInfo = lootInfo;
        panel.duration = duration;
        panel.spriteRenderer.sprite = lootInfo.item.value.icon;
        panel.text.text = lootInfo.amount.ToString();
        return panel;
    }

    private void Start()
    {
        createdTime = Time.realtimeSinceStartup;
    }

    public void Update()
    {
        if (Time.realtimeSinceStartup - createdTime > duration)
        {
            Destroy(this.gameObject);
            return;
        }

        transform.LookAt(Camera.main.transform.position);
    }
}