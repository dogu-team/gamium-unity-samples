using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    // potion-related UI elements from GameScreen

    [RequireComponent(typeof(UIDocument))]
    public class PotionScreen : MonoBehaviour
    {
        public static event Action<VisualElement> SlotHealed;

        [Tooltip("Check to visualize healing potion drop slots when dragging.")]
        [SerializeField] bool m_IsSlotVisible;

        // element string IDs
        const string k_DragArea = "healing-potion__drag-area";
        const string k_StartDrag = "healing-potion__space";
        const string k_PointerIcon = "healing-potion__image";

        const string k_SlotClass = "healing-potion__slot";
        const string k_HealPotionCount = "healing-potion__count";

        const string k_PotionIconActiveClass = "potion--active";
        const string k_PotionIconInactiveClass = "potion--inactive";

        // game screen document
        UIDocument m_Document;

        // draggable portion of the screen area
        VisualElement m_DragArea;

        // element to begin dragging
        VisualElement m_StartElement;

        // potion image that acts as pointer
        VisualElement m_PointerIcon;

        // "dropzone" regions marked for each character
        List<VisualElement> m_HealDropZones;
        VisualElement m_ActiveZone;

        Label m_HealPotionCount;

        // is the pointer currently active
        bool m_IsDragging;

        // is one or more potions left?
        bool m_IsPotionAvailable;

        // used to calculate offset between potion icon and mouse pointer
        Vector3 m_IconStartPosition;
        Vector3 m_PointerStartPosition;

        void OnEnable()
        {
            PotionData.PotionsUpdated += OnPotionsUpdated;
        }

        void OnDisable()
        {
            PotionData.PotionsUpdated -= OnPotionsUpdated;
        }
        void SetVisualElements()
        {
            m_Document = GetComponent<UIDocument>();
            VisualElement rootElement = m_Document.rootVisualElement;

            m_DragArea = rootElement.Q<VisualElement>(k_DragArea);
            m_StartElement = rootElement.Q<VisualElement>(k_StartDrag);
            m_PointerIcon = rootElement.Q<VisualElement>(k_PointerIcon);
            m_HealPotionCount = rootElement.Q<Label>(k_HealPotionCount);

            m_HealDropZones = rootElement.Query<VisualElement>(className: k_SlotClass).ToList();
            HideSlots();
        }

        void HideSlots()
        {
            foreach (VisualElement slot in m_HealDropZones)
            {
                slot.style.opacity = 0f;
            }
        }

        void Awake()
        {
            if (m_Document == null)
                m_Document = GetComponent<UIDocument>();

            SetVisualElements();
            RegisterCallbacks();

        }
        void Start()
        {
            m_DragArea.style.display = DisplayStyle.None;
        }

        void RegisterCallbacks()
        {
            // listen for LMB down
            m_StartElement.RegisterCallback<PointerDownEvent>(PointerDownEventHandler);

            // listen for mouse movement and LMB up
            m_DragArea.RegisterCallback<PointerMoveEvent>(PointerMoveEventHandler);
            m_DragArea.RegisterCallback<PointerUpEvent>(PointerUpEventHandler);

            // raise events when entering or leaving "slot" above each character
            foreach (VisualElement slot in m_HealDropZones)
            {
                slot.RegisterCallback<PointerEnterEvent>(PointerEnterEventHandler);
                slot.RegisterCallback<PointerLeaveEvent>(PointerLeaveEventHandler);
            }
        }

        // pointer leaves an active character dropslot
        void PointerLeaveEventHandler(PointerLeaveEvent evt)
        {
            // unset the active slot
            if (m_IsDragging)
            {
                VisualElement currentTarget = evt.currentTarget as VisualElement;
                if (m_ActiveZone == currentTarget)
                {
                    m_ActiveZone.style.opacity = 0f;
                    m_ActiveZone = null;
                }
            }
        }

        void PointerEnterEventHandler(PointerEnterEvent evt)
        {
            // set the active slot
            if (m_IsDragging)
            {
                m_ActiveZone = evt.currentTarget as VisualElement;

                // show the slot (or keep it invisible)
                if (m_IsSlotVisible)
                    m_ActiveZone.style.opacity = 0.1f;
            }
        }

        void PointerMoveEventHandler(PointerMoveEvent evt)
        {
            // offset icon to the current pointer position if active
            if (m_IsDragging && m_DragArea.HasPointerCapture(evt.pointerId))
            {
                float newX = m_IconStartPosition.x + (evt.position.x - m_PointerStartPosition.x);
                float newY = m_IconStartPosition.y + (evt.position.y - m_PointerStartPosition.y);

                m_PointerIcon.transform.position = new Vector2(newX, newY);
            }
        }

        void PointerDownEventHandler(PointerDownEvent evt)
        {

            if (!m_IsPotionAvailable)
                return;

            // enable the drag area and hides the slots
            m_DragArea.style.display = DisplayStyle.Flex;
            HideSlots();

            // send all pointer events to the DragArea element
            m_DragArea.CapturePointer(evt.pointerId);

            // set the icon and pointer starting positions
            m_IconStartPosition = m_PointerIcon.transform.position;
            m_PointerStartPosition = evt.position;

            m_IsDragging = true;
        }

        void PointerUpEventHandler(PointerUpEvent evt)
        {
            // disable the drag area and release the pointer
            m_DragArea.style.display = DisplayStyle.None;
            m_DragArea.ReleasePointer(evt.pointerId);
            m_IsDragging = false;

            // restore the potion icon
            m_PointerIcon.transform.position = m_IconStartPosition;

            // send a message with the selected slot
            if (m_ActiveZone != null)
            {
                // notify the GameHealDrop components and reset
                SlotHealed?.Invoke(m_ActiveZone);
                m_ActiveZone = null;
            }
        }

        // event-handling methods
        void OnPotionsUpdated(int potionCount)
        {

            m_IsPotionAvailable = (potionCount > 0);
            EnablePotionIcon(m_IsPotionAvailable);
            m_HealPotionCount.text = potionCount.ToString();
        }

        void EnablePotionIcon(bool state)
        {
            if (state)
            {
                m_PointerIcon.RemoveFromClassList(k_PotionIconInactiveClass);
                m_PointerIcon.AddToClassList(k_PotionIconActiveClass);
            }
            else
            {
                m_PointerIcon.RemoveFromClassList(k_PotionIconActiveClass);
                m_PointerIcon.AddToClassList(k_PotionIconInactiveClass);
            }
        }

    }
}
