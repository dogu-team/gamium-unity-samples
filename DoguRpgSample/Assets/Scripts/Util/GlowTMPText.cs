using System;
using TMPro;
using UnityEngine;

namespace Util
{
    public class GlowTMPText : MonoBehaviour
    {
        private TMP_Text text;

        private void Start()
        {
            text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.PingPong(Time.time, 0.7f));
        }
    }
}