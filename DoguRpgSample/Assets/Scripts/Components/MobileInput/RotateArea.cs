using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileInput
{
    public class RotateArea : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
    {
        [NonSerialized] public Vector2 moveAxis;
        private RectTransform thisRect;
        private Vector2 startPos;
        private Vector2 currentPos;
        private float moveScale = 0.01f;

        private void Awake()
        {
            thisRect = GetComponent<RectTransform>();

            Reposition();
            ScreenRect.Instance.OnResized((rect) =>
            {
                thisRect.sizeDelta = new Vector2((int)(rect.width / 2), rect.height);
                Reposition();
            });
        }

        public void Reposition()
        {
            moveAxis = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRect, eventData.position,
                eventData.pressEventCamera, out startPos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRect, eventData.position,
                eventData.pressEventCamera, out currentPos);
            var direction = currentPos - startPos;
            moveAxis = (direction * moveScale);
            moveAxis.x = Mathf.Clamp(moveAxis.x, -1, 1);
            moveAxis.y = Mathf.Clamp(moveAxis.y, -1, 1);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Reposition();
        }
    }
}