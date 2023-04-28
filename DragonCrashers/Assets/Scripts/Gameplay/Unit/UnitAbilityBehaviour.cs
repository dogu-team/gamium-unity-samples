using System.Collections;
using System.Collections.Generic;
using UIToolkitDemo;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Utilities.Inspector;
using System;



public class UnitAbilityBehaviour : MonoBehaviour
{
    public enum QueueActionAfterCooldown
    {
        Automatic,
        ManualButton
    }

    [Header("Settings")]
    public UnitAbilityData data;
    public QueueActionAfterCooldown queueActionAfterCooldown;

    [Header("Runtime ID")]
    [SerializeField]
    [ReadOnly] private int ID;

    //Cooldown Timer
    public DurationTimer cooldownTimer;
    private HealthBarComponent m_CoolDownMeter;

    [Header("States")]

    [SerializeField]
    [ReadOnly] public bool cooldownActive;

    [SerializeField]
    [ReadOnly] public bool abilityReady;

    private bool waitToBeAddedToQueue;

    [Header("Ability Timeline")]
    public PlayableDirector abilityTimeline;

    [Header("Events")]
    public UnityEvent<int> abilityReadyForQueue;
    public UnityEvent<int, TargetType> applyAbilityValueToTargets;
    public UnityEvent<int> abilitySequenceFinished;

    //Delegate for external systems to detect (IE: Unit's UI)
    public delegate void AbilityCooldownChangedEventHandler(float newCooldownAmount);
    public event AbilityCooldownChangedEventHandler AbilityCooldownChangedEvent;

    // from original Dragon Crashers, unused
    public delegate void AbilityReadyEventHandler();
    public event AbilityReadyEventHandler AbilityReadyEvent;

    const float k_dischargeDelay = 1f;
    public static Action<UnitAbilityBehaviour> AbilityCharged;
    public static Action<UnitAbilityBehaviour> AbilityDischarged;

    public void SetupID(int newID)
    {
        ID = newID;
    }

    public void SetupAbilityCooldownTimer()
    {
        cooldownTimer = new DurationTimer(data.cooldownTime);
    }


    public void StartAbilityCooldown(HealthBarComponent progressBar = null)
    {
        cooldownActive = true;
        abilityReady = false;
        if (progressBar != null)
        {
            m_CoolDownMeter = progressBar;
            m_CoolDownMeter.MaximumHealth = Mathf.RoundToInt(data.cooldownTime);
        }
    }

    void Update()
    {
        CheckAbilityCooldown();
    }

    void CheckAbilityCooldown()
    {
        if (cooldownActive)
        {
            cooldownTimer.UpdateTimer();
            DelegateEventAbilityCooldownChanged();

            if (m_CoolDownMeter != null)
            {
                m_CoolDownMeter.CurrentHealth = Mathf.RoundToInt(cooldownTimer.GetPolledTime());
            }

            if (cooldownTimer.HasElapsed())
            {
                cooldownTimer.EndTimer();
                cooldownTimer.Reset();
                AbilityCooldownFinished();
                return;
            }

        }
    }

    void AbilityCooldownFinished()
    {
        cooldownActive = false;
        abilityReady = true;

        // notify frame fx
        AbilityCharged?.Invoke(this);

        switch (queueActionAfterCooldown)
        {
            case QueueActionAfterCooldown.Automatic:
                AddAbilityToQueue();
                break;

            case QueueActionAfterCooldown.ManualButton:
                // original Dragon Crashers, unused here
                DelegateEventAbilityReady();
                break;
        }
    }

    public void AddAbilityToQueue()
    {
        if (abilityReady)
        {
            abilityReadyForQueue.Invoke(ID);
        }
    }

    public void BeginAbilitySequence()
    {
        abilityTimeline.Play();
        abilityReady = false;
        
    }

    public void AbilityMarkerHappened()
    {
        int abilityValue = data.GetRandomValueInRange();
        applyAbilityValueToTargets.Invoke(abilityValue, data.targetType);
    }

    public void AbilitySequenceFinished()
    {
        cooldownActive = true;
        abilitySequenceFinished.Invoke(ID);

        // turn off frame fx and reset the cooldown meter
        AbilityDischarged?.Invoke(this);
        if (m_CoolDownMeter != null)
        {
            m_CoolDownMeter.CurrentHealth = 0;
        }
    }

    public void StopAbility()
    {
        cooldownActive = false;
        abilityReady = false;
    }

    void DelegateEventAbilityCooldownChanged()
    {
        if (AbilityCooldownChangedEvent != null)
        {
            AbilityCooldownChangedEvent(cooldownTimer.GetPolledTime());
        }
    }

    void DelegateEventAbilityReady()
    {
        if (AbilityReadyEvent != null)
        {
            AbilityReadyEvent();
        }
    }

}
