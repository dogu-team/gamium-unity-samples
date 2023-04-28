using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    // overlay texture for reference; disable when not in use
    [ExecuteInEditMode]
    [RequireComponent(typeof(UIDocument))]
    public class ReferenceScreen : MonoBehaviour
    {
        UIDocument document;
        VisualElement m_Root;


        [SerializeField] [Range(0f, 1f)] float m_Opacity = 0.5f;
        public bool disableOnPlay = true;

        void OnEnable()
        {
            document = GetComponent<UIDocument>();
            m_Root = document.rootVisualElement;

            // overlay on top
            document.sortingOrder = 9999;
        }

        private void Start()
        {
            if (disableOnPlay)
            {
                gameObject.SetActive(false);
            }
        }
        private void Update()
        {
            if (m_Root != null)
                m_Root.style.opacity = m_Opacity;
        }
    }
}
