using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Components
{
    public class MultipurposePopup : MonoBehaviour
    {
        public enum PopupButtonType
        {
            Close,
            OkCancel,
        }

        public class PopupParam
        {
            public Sprite icon;
            public string text = string.Empty;
            public string negativeButtonText = string.Empty;
            public string positiveButtonText = string.Empty;
            public bool hasCounter;
            public PopupButtonType buttonType;
            public Action onCancel;
            public Action<int> onConfirm;
        }

        public RectTransform iconPanel;
        public Image icon;
        public Text text;
        public RectTransform counter;
        public Text negativeText;
        public Text positiveText;
        public Button closeButton;
        public Button confirmButton;
        private Action onCancel;
        private Action<int> onConfirm;
        private int count = 1;

        public void Initialize(PopupParam param)
        {
            if (null == icon)
            {
                iconPanel.gameObject.SetActive(false);
            }
            else
            {
                iconPanel.gameObject.SetActive(true);
                icon.sprite = param.icon;
            }

            text.text = param.text;

            if (!param.hasCounter)
            {
                counter.gameObject.SetActive(false);
            }
            else
            {
                counter.gameObject.SetActive(true);
            }

            switch (param.buttonType)
            {
                case PopupButtonType.Close:
                    negativeText.text = string.IsNullOrEmpty(param.negativeButtonText)
                        ? "Close"
                        : param.negativeButtonText;
                    closeButton.gameObject.SetActive(true);
                    confirmButton.gameObject.SetActive(false);
                    closeButton.onClick.AddListener(OnCancel);
                    break;
                case PopupButtonType.OkCancel:
                    negativeText.text = string.IsNullOrEmpty(param.negativeButtonText)
                        ? "Cancel"
                        : param.negativeButtonText;
                    positiveText.text = string.IsNullOrEmpty(param.positiveButtonText)
                        ? "Confirm"
                        : param.positiveButtonText;
                    closeButton.gameObject.SetActive(true);
                    confirmButton.gameObject.SetActive(true);
                    closeButton.onClick.AddListener(OnCancel);
                    confirmButton.onClick.AddListener(OnConfirm);
                    break;
            }

            onCancel = param.onCancel;
            onConfirm = param.onConfirm;
        }

        public void OnCancel()
        {
            onCancel?.Invoke();
            DestroySelf();
        }

        public void OnConfirm()
        {
            onConfirm?.Invoke(count);
            DestroySelf();
        }

        public void OnCountIncreased()
        {
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}