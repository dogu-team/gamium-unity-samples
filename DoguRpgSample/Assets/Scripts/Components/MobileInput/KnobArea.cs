using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileInput
{
    public class KnobArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public RectTransform knobStart;
        public RectTransform knobCurrent;
        public RectTransform line;
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
                thisRect.sizeDelta = new Vector2((int)(rect.width / 3), thisRect.sizeDelta.y);
                Reposition();
            });
        }

        public void Reposition()
        {
            knobCurrent.anchoredPosition = new Vector2(thisRect.sizeDelta.x / 2, thisRect.sizeDelta.y / 2);
            knobStart.gameObject.SetActive(false);
            knobCurrent.gameObject.SetActive(true);
            line.gameObject.SetActive(false);
            moveAxis = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRect, eventData.position,
                eventData.pressEventCamera, out startPos);
            knobCurrent.anchoredPosition = startPos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRect, eventData.position,
                eventData.pressEventCamera, out currentPos);
            knobStart.gameObject.SetActive(true);
            knobCurrent.anchoredPosition = currentPos;
            knobStart.anchoredPosition = startPos;
            var direction = currentPos - startPos;
            line.anchoredPosition = (startPos + currentPos) / 2;
            line.sizeDelta = new Vector2(direction.magnitude, line.sizeDelta.y);
            line.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            line.gameObject.SetActive(true);
            moveAxis = (direction * moveScale);
            moveAxis.x = Mathf.Clamp(moveAxis.x, -1, 1);
            moveAxis.y = Mathf.Clamp(moveAxis.y, -1, 1);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Reposition();
        }
    }
}