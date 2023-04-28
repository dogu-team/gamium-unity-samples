using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobileInput
{
    public class MobileInputController : MonoBehaviour
    {
        public KnobArea knobArea;
        public RotateArea rotateArea;
        [FormerlySerializedAs("buttonArea")] public RingButtonArea ringButtonArea;

        public float Horizontal =>
            knobArea.moveAxis.x;

        public float Vertical =>
            knobArea.moveAxis.y;

        public float Yaw =>
            rotateArea.moveAxis.x;

        public float Pitch =>
            rotateArea.moveAxis.y;

        private void OnDisable()
        {
            knobArea.Reposition();
            rotateArea.Reposition();
        }

        public bool GetButton(string buttonName)
        {
            return ringButtonArea.GetButton(buttonName);
        }

        public bool GetButtonDown(string buttonName)
        {
            return ringButtonArea.GetButtonDown(buttonName);
        }
    }
}