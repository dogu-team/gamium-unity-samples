using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ChartLibrary;

namespace UIToolkitDemo
{
    // Shows the player's total experience level (the sum of all four character levels)
    public class LevelMeter : MenuScreen
    {
        // level meter (top)
        const string k_LevelMeterNumber = "level-meter__number";
        const string k_LevelMeterCounter = "level-meter__counter";

        // time to show radial progress bar
        const float lerpTime = 1f;

        RadialCounter m_LevelMeterCounter;
        Label m_LevelMeterNumber;

        void OnEnable()
        {
            // listen for CharScreenController events
            CharScreenController.LevelUpdated += OnLevelUpdated;
        }

        void OnDisable()
        {
            CharScreenController.LevelUpdated -= OnLevelUpdated;
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            m_LevelMeterCounter = m_Root.Q<RadialCounter>(k_LevelMeterCounter);
            m_LevelMeterNumber = m_Root.Q<Label>(k_LevelMeterNumber);
        }

        // Level Meter
        void OnLevelUpdated(float progress)
        {
            StartCoroutine(SetCounterRoutine(progress, lerpTime));
        }

        IEnumerator SetCounterRoutine(float targetValue, float lerpTime)
        {
            float t = 0;
            float originalValue = m_LevelMeterCounter.progress;
            float tolerance = 0.05f;
            m_LevelMeterNumber.text = targetValue.ToString();

            while (Mathf.Abs(m_LevelMeterCounter.progress - targetValue) > tolerance)
            {
                m_LevelMeterCounter.progress = Mathf.Lerp(originalValue, targetValue, t);
                yield return null;

                // increment based on lerpTime duration
                t += Time.deltaTime / lerpTime;
            }
            m_LevelMeterCounter.progress = targetValue;

        }
    }
}
