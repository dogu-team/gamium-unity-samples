using System;
using Components;
using Data.Static;
using MobileInput;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameSceneView : MonoBehaviour
    {
        public TextBar hpBar;
        public TextBar mpBar;
        public Text levelText;
        public Text expPercentText;
        public Animator autoHuntAnimator;
        private bool isAutoHunt = false;
        private GameManager gameManager;

        private void Start()
        {
            isAutoHunt = false;
            RefreshAnimation();

            gameManager = FindObjectOfType<GameManager>();

            var playerCharacterController = FindObjectOfType<PlayerCharacterController>();
            var stats = playerCharacterController.actionController.stat;
            var health = stats.Get(Stat.StatType.Health);
            hpBar.slider.maxValue = health.ModifiersValue;
            hpBar.slider.value = health.Value;
            hpBar.Refresh();
            health.onValueChanged = (health) =>
            {
                hpBar.slider.maxValue = health.ModifiersValue;
                hpBar.slider.value = health.Value;
                hpBar.Refresh();
            };

            var mana = stats.Get(Stat.StatType.Mana);
            mpBar.slider.maxValue = mana.ModifiersValue;
            mpBar.slider.value = mana.Value;
            mpBar.Refresh();
            mana.onValueChanged = (mana) =>
            {
                mpBar.slider.maxValue = mana.ModifiersValue;
                mpBar.slider.value = mana.Value;
                mpBar.Refresh();
            };

            var exp = stats.Get(Stat.StatType.Experience);
            var currentCharacter = PlayerDataController.Instance.GetCurrentPlayerCharacter();
            currentCharacter.GetLevelInfo(out var levelInfo, out var nextLevelInfo);

            levelText.text = levelInfo.level.ToString();
            expPercentText.text = currentCharacter.GetExpPercent().ToString();

            exp.onValueChanged = (expValue) =>
            {
                var currentCharacter = PlayerDataController.Instance.GetCurrentPlayerCharacter();
                currentCharacter.Experience = expValue.Value;
                currentCharacter.GetLevelInfo(out var levelInfo, out var nextLevelInfo);

                levelText.text = levelInfo.level.ToString();
                expPercentText.text = currentCharacter.GetExpPercent().ToString();

                PlayerDataController.Instance.SavePlayerCharacter();
            };
        }

        private void OnEnable()
        {
            RefreshAnimation();
        }

        public void ShowGameUI()
        {
            gameManager.mobileInputController.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }

        public void HideGameUI()
        {
            gameManager.mobileInputController.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }


        public void OnAutoHuntClicked()
        {
            isAutoHunt = !isAutoHunt;
            AutoHuntController.Instance.SetAutoHunt(isAutoHunt);
            RefreshAnimation();
        }

        private void RefreshAnimation()
        {
            if (!isAutoHunt)
            {
                autoHuntAnimator.StartPlayback();
            }
            else
            {
                autoHuntAnimator.StopPlayback();
            }
        }
    }
}