using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class BlurImage : MonoBehaviour
    {
        public float radius = 1.0f;
        private Material material;
        private static readonly int Radius = Shader.PropertyToID("_Radius");

        private void Start()
        {
            var image = GetComponent<Image>();
            material = image.material;
            // material = new Material(Shader.Find("Hidden/ImageBlurShader")); 
        }

        private void Update()
        {
            material.SetFloat(Radius, radius);
        }
    }
}