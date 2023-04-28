using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace MobileInput
{
    public class MobileUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public string buttonName;
        public bool isDown { get; private set; }
        public int downFrame { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            downFrame = Time.frameCount + 1;
            isDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
        }
    }
}