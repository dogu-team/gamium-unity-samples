using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class UnitsDamageDisplayListener : MonoBehaviour
    {
        
        [Header("Damage Data Sources")]
        public UnitDamageDisplayBehaviour[] unitDamageDisplayBehaviours;

        [Header("Display Managers")]
        public NumberDisplayManager numberDisplayManager;
        public HitVFXDisplayManager hitVFXDisplayManager;

        void OnEnable()
        {
            for(int i = 0; i < unitDamageDisplayBehaviours.Length; i++)
            {
                unitDamageDisplayBehaviours[i].DamageDisplayEvent += ShowDamageDisplays;
            }
        }

        void OnDisable()
        {
            for(int i = 0; i < unitDamageDisplayBehaviours.Length; i++)
            {
                unitDamageDisplayBehaviours[i].DamageDisplayEvent -= ShowDamageDisplays;
            }
        }

        void ShowDamageDisplays(int damageAmount, Transform damageLocation, Color damageColor)
        {
            numberDisplayManager.ShowNumber(damageAmount, damageLocation, damageColor);
            hitVFXDisplayManager.ShowHitVFX(damageLocation);
        }

    }  
