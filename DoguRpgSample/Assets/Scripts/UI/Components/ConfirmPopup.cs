using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class ConfirmPopup : MonoBehaviour
    {
        public Text text;
        [NonSerialized] public Action onCancel;
        [NonSerialized] public Action onConfirm;

        public void OnCancel()
        {
            onCancel?.Invoke();
            DestroySelf();
        }

        public void OnConfirm()
        {
            onConfirm?.Invoke();
            DestroySelf();
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}