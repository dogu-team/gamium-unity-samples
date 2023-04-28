using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    // example utility to convert a Visual Element center position to a world space position

    // note:
    //          this is unused in the demo but may be helpful when relating Visual Elements to GameObjects
    //          the PanelSettings asset contains Scale Mode Parameters for matching different output resolutions
    //          use a Reference Resolution that matches the output for ideal results (demo defaults to full HD, or 1920 x 1080)
    //          
    public class ElementToWorldPosition : MonoBehaviour
    {
        [SerializeField] UIDocument m_Document;
        
        [Tooltip("Name of Visual Element")]
        [SerializeField] string m_ElementID;

        [SerializeField] Camera m_Camera;
        [SerializeField] GameObject m_TargetObject;
        [SerializeField] float m_ZDepth = 10f;
        [SerializeField] bool m_UpdateEveryFrame;

        VisualElement m_TargetElement;

        void Start()
        {
            m_Document.rootVisualElement.RegisterCallback<GeometryChangedEvent>(Setup);
            m_TargetElement = m_Document.rootVisualElement.Q<VisualElement>(m_ElementID);
        }

        void Setup(GeometryChangedEvent evt)
        {
            m_TargetObject.transform.position = m_TargetElement.GetWorldPosition(m_Camera, m_ZDepth);
        }

        void Update()
        {
            if (m_UpdateEveryFrame)
            {
                m_TargetObject.transform.position = m_TargetElement.GetWorldPosition(m_Camera, m_ZDepth);
            }
        }
    }
}
