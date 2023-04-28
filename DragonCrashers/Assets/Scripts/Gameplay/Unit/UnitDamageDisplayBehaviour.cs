using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitDamageDisplayBehaviour : MonoBehaviour
{

    [Header("Damage Color Tint")]
    public Color damageColorTint = Color.red;

    [Header("Heal Color Tint")]
    public Color healColorTint = Color.green;

    [Header("Damage Location")]
    public Transform damageDisplayTransform;

    public delegate void DamageDisplayEventHandler(int newDamageAmount, Transform displayLocation, Color damageColor);
    public event DamageDisplayEventHandler DamageDisplayEvent;


    public void DisplayDamage(int damageTaken)
    {
        if (DamageDisplayEvent != null)
        {
            // tint colors for damage or healing
            Color colorTint = (damageTaken < 0) ? damageColorTint : healColorTint;
            DamageDisplayEvent(damageTaken, damageDisplayTransform, colorTint);
        }
    }

}
