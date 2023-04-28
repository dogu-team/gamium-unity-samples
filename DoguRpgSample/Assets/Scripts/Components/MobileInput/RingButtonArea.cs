using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MobileInput
{
    public class RingButtonArea : MonoBehaviour
    {
        private MobileUIButton[] buttons;
        private Dictionary<string, MobileUIButton> buttonMap = new Dictionary<string, MobileUIButton>();

        private void Awake()
        {
            buttons = GetComponentsInChildren<MobileUIButton>();
            foreach (var button in buttons)
            {
                buttonMap.Add(button.buttonName, button);
            }
        }

        public bool GetButton(string buttonName)
        {
            if (!buttonMap.ContainsKey(buttonName)) return false;
            return buttonMap[buttonName].isDown;
        }

        public bool GetButtonDown(string buttonName)
        {
            if (!buttonMap.ContainsKey(buttonName)) return false;
            return buttonMap[buttonName].isDown && buttonMap[buttonName].downFrame == Time.frameCount;
        }
    }
}