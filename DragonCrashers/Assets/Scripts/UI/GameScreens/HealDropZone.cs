using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    [RequireComponent(typeof(UnitController))]
    public class HealDropZone : MonoBehaviour
    {
        [Tooltip("Represents Health Potion drop area over each character.")]
        [SerializeField] string m_SlotID;
        [SerializeField] UIDocument m_GameScreenDocument;

        [Range(1,100)]
        [SerializeField] float m_PercentHealthBoost;

        VisualElement m_Slot;
        UnitController m_UnitController;
        UnitHealthBehaviour m_UnitHealth;

        int m_MaxHealth;
        int m_HealthBoost;

        public static Action UseOnePotion;

        void OnEnable()
        {
            PotionScreen.SlotHealed += OnSlotHealed;
            UnitController.UnitDied += OnUnitDied;
        }

        void OnDisable()
        {
            PotionScreen.SlotHealed -= OnSlotHealed;
            UnitController.UnitDied -= OnUnitDied;
        }
        void Start()
        {
            m_UnitController = GetComponent<UnitController>();
            m_UnitHealth = m_UnitController.healthBehaviour;
            m_MaxHealth = m_UnitController.data.totalHealth;
            m_HealthBoost = (int)(m_MaxHealth * m_PercentHealthBoost / 100f);
            SetVisualElements();
        }
        void SetVisualElements()
        {
            VisualElement rootElement = m_GameScreenDocument.rootVisualElement;
            m_Slot = rootElement.Query<VisualElement>(m_SlotID);
            EnableSlot(true);
        }

        void EnableSlot(bool state)
        {
            if (m_Slot == null)
                return;

            m_Slot.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
        }

        // event-handling methods
        void OnSlotHealed(VisualElement activeSlot)
        {
            // healed the associated unit
            if (activeSlot == m_Slot)
            {
                m_UnitHealth?.ChangeHealth(m_HealthBoost);
                UseOnePotion?.Invoke();
            }
        }

        // disable healing slots for dead units
        void OnUnitDied(UnitController deadUnit)
        {
            if (deadUnit == m_UnitController)
            {
                EnableSlot(false);
            }
        }
    }
}
