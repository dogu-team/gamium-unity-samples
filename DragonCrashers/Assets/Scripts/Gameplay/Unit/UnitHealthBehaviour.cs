using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Inspector;
using System;

public class UnitHealthBehaviour : MonoBehaviour
{

    [Header("Health Info")]
    [SerializeField]
    [ReadOnly] private int currentHealth;

    [Header("Events")]
    public UnityEvent<int> healthDifferenceEvent;
    public UnityEvent healthIsZeroEvent;

    // pass current health to external listeners (UnitController)
    public Action<int> HealthChanged;

    int m_TotalHealth;

    public void SetupCurrentHealth(int totalHealth)
    {
        currentHealth = totalHealth;
        m_TotalHealth = totalHealth;
    }

    public void ChangeHealth(int healthDifference)
    {
        currentHealth = Mathf.Clamp( currentHealth + healthDifference, 0, m_TotalHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HealthIsZeroEvent();
        }

        // UnityEvent for damage
        healthDifferenceEvent.Invoke(healthDifference);

        // event to pass current health
        HealthChanged?.Invoke(currentHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    void HealthIsZeroEvent()
    {
        healthIsZeroEvent.Invoke();
    }

}
