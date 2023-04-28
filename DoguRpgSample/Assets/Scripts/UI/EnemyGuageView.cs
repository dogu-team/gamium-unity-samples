using System;
using UI.Components;
using UnityEngine;

namespace UI
{
    public class EnemyGuageView : MonoBehaviour
    {
        public TextBar hpBar;
        [NonSerialized] public GameObject target;

        private void Update()
        {
            var screenPoint = Camera.main.WorldToScreenPoint(target.transform.position);
            transform.position = screenPoint;
        }
    }
}