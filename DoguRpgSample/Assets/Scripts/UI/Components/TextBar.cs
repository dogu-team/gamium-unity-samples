using System;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Components
{
    public class TextBar : MonoBehaviour
    {
        public Slider slider;
        public Text leftText;
        public Text rightText;
        public Image fillImage;
        public Color fillColor;

        public void Refresh()
        {
            leftText.text = ((int)slider.value).ToString();
            rightText.text = ((int)slider.maxValue).ToString();
        }

        private void OnDrawGizmosSelected()
        {
            fillImage.color = fillColor;
            Refresh();
        }
    }
}