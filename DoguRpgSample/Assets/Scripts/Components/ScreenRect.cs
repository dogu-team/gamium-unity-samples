using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class ScreenRect : SingletonMonoBehavior<ScreenRect>
{
    private Rect? rect;
    private int updateCount = 0;


    void Awake()
    {
        var canvas = this.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var canvasScaler = this.gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.matchWidthOrHeight = 1;
    }

    private void Update()
    {
        updateCount += 1;
        if (1 < updateCount)
        {
            rect = this.gameObject.GetComponent<RectTransform>().rect;
        }
    }

    public void OnResized(Action<Rect> action)
    {
        TaskManager.Instance.StartCoroutine(NotifyResize(action));
    }

    private IEnumerator NotifyResize(Action<Rect> action)
    {
        while (null == rect)
        {
            yield return null;
        }

        action?.Invoke(rect.Value);
    }
}