using System;
using System.Collections;
using System.Collections.Generic;
using UIToolkitDemo;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Inspector;

public class UnitController : MonoBehaviour
{

    [Header("Data")]
    public UnitInfoData data;

    [Header("Health Settings")]
    public UnitHealthBehaviour healthBehaviour;
    private bool unitIsAlive;
    public HealthBarController healthBarBehaviour;

    [Header("Target Settings")]
    public UnitTargetsBehaviour targetsBehaviour;

    [Header("Ability Settings")]
    public UnitAbilitiesBehaviour abilitiesBehaviour;

    [Header("Animation Settings")]
    public UnitCharacterAnimationBehaviour characterAnimationBehaviour;

    [Header("Audio Settings")]
    public UnitAudioBehaviour audioBehaviour;

    [Header("UI")]
    [SerializeField] private GameScreen m_GameScreen;
    [SerializeField] private VisualTreeAsset m_CharacterVisualTree;

    [Header("Debug")]
    public bool initializeSelf;

    public delegate void UnitDiedEventHandler(UnitController unit);
    public event UnitDiedEventHandler UnitDiedEvent;

    public static Action<UnitController> SpecialCharged;
    public static Action<UnitController> SpecialDischarged;
    public static Action<UnitController> UnitDied;

    private UnitAbilityBehaviour m_SpecialAbility;
    private CharacterCard m_CharacterCard;
    public CharacterCard CharacterCard => m_CharacterCard;
    void Start()
    {
        if (initializeSelf)
        {
            SetAlive();
            BattleStarted();
        }

    }

    void OnEnable()
    {
        if (healthBehaviour != null)
            healthBehaviour.HealthChanged += OnHealthChanged;

        GameScreen.GamePaused += OnGamePaused;
        GameScreen.GameResumed += OnGameResumed;

        UnitAbilityBehaviour.AbilityCharged += OnAbilityCharged;
        UnitAbilityBehaviour.AbilityDischarged += OnAbilityDischarged;

    }

    void OnDisable()
    {
        if (healthBehaviour != null)
            healthBehaviour.HealthChanged -= OnHealthChanged;

        GameScreen.GamePaused -= OnGamePaused;
        GameScreen.GameResumed -= OnGameResumed;

        UnitAbilityBehaviour.AbilityCharged += OnAbilityCharged;
        UnitAbilityBehaviour.AbilityDischarged += OnAbilityDischarged;
    }

    // event-handling methods
    void OnGamePaused(float delay)
    {
        healthBarBehaviour?.DisplayHealthBar(false);
    }

    void OnGameResumed()
    {

        healthBarBehaviour?.DisplayHealthBar(true);
    }

    void OnHealthChanged(int newCurrentHealth)
    {
        if (unitIsAlive)
            healthBarBehaviour.UpdateHealth(newCurrentHealth);
    }

    // notify GameScreen to enable FrameFX
    void OnAbilityCharged(UnitAbilityBehaviour ability)
    {
        if (ability == m_SpecialAbility)
        {
            //Debug.Log(ability.data.abilityName + " charged on unit " + this.data.unitName);
            SpecialCharged?.Invoke(this);
        }
    }

    // notify GameScreen to disable FrameFX
    void OnAbilityDischarged(UnitAbilityBehaviour ability)
    {
        if (ability == m_SpecialAbility)
        {
            //Debug.Log(ability.data.abilityName + " discharged on unit " + this.data.unitName);
            SpecialDischarged?.Invoke(this);
        }
    }



    public void AssignTargetUnits(List<UnitController> units)
    {
        targetsBehaviour.AddTargetUnits(units);
    }

    public void RemoveTargetUnit(UnitController unit)
    {
        targetsBehaviour.RemoveTargetUnit(unit);
    }

    public void SetAlive()
    {
        healthBehaviour?.SetupCurrentHealth(data.totalHealth);
        healthBarBehaviour?.DisplayHealthBar(true);
        healthBarBehaviour?.SetHealth(data.totalHealth, data.totalHealth);

        unitIsAlive = true;


        if (m_GameScreen != null && m_CharacterVisualTree != null)
        {
            m_CharacterCard = gameObject.AddComponent<CharacterCard>();
            m_CharacterCard.CharacterVisualTreeAsset = m_CharacterVisualTree;
            m_CharacterCard.HeroData = data;
            m_GameScreen.AddHero(m_CharacterCard);

            // designate the last Ability as the "special" -- for the UI counter
            if (abilitiesBehaviour != null)
            {
                m_SpecialAbility = abilitiesBehaviour.abilities[abilitiesBehaviour.abilities.Length - 1];
                //Debug.Log("Special Ability for " + data.unitName + " is " + m_SpecialAbility.data.abilityName);
            }

        }
    }

    public void BattleStarted()
    {
        if (m_GameScreen != null && m_CharacterVisualTree != null)
        {
            abilitiesBehaviour.StartAbilityCooldowns(m_CharacterCard.CooldownBar);
        }
        else
        {
            abilitiesBehaviour.StartAbilityCooldowns();
        }
    }

    public void BattleEnded()
    {
        abilitiesBehaviour.StopAllAbilities();
    }

    public void AbilityHappened(int abilityValue, TargetType unitTargetType)
    {
        List<UnitController> targetUnits = targetsBehaviour.FilterTargetUnits(unitTargetType);

        if (targetUnits.Count > 0)
        {
            for (int i = 0; i < targetUnits.Count; i++)
            {
                targetUnits[i].ReceiveAbilityValue(abilityValue);
            }
        }
    }

    public void ReceiveAbilityValue(int abilityValue)
    {
        if (unitIsAlive)
        {
            healthBehaviour.ChangeHealth(abilityValue);
            characterAnimationBehaviour.CharacterWasHit();

            healthBarBehaviour.UpdateHealth(GetCurrentHealth());
        }
    }

    public int GetCurrentHealth()
    {
        return healthBehaviour.GetCurrentHealth();
    }

    public void UnitHasDied()
    {
        unitIsAlive = false;

        healthBarBehaviour?.DisplayHealthBar(false);

        abilitiesBehaviour.StopAllAbilities();
        characterAnimationBehaviour.CharacterHasDied();

        UnitDied?.Invoke(this);

        // notify BattleGameManager
        DelegateEventUnitDied();
    }

    void DelegateEventUnitDied()
    {
        if (UnitDiedEvent != null)
        {
            UnitDiedEvent(this);
        }
    }
}
