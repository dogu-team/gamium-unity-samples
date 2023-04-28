using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;

namespace UIToolkitDemo
{
    [RequireComponent(typeof(UIDocument))]
    public class GameScreen : MonoBehaviour
    {
        // notify listeners to pause after delay in seconds
        public static event Action<float> GamePaused;
        public static event Action GameResumed;
        public static event Action GameQuit;
        public static event Action GameRestarted;
        public static event Action<float> MusicVolumeChanged;
        public static event Action<float> SfxVolumeChanged;

        [Header("Menu Screen elements")][Tooltip("String IDs to query Visual Elements")]
        [SerializeField] string m_PauseScreenName = "PauseScreen";
        [SerializeField] string m_WinScreenName = "GameWinScreen";
        [SerializeField] string m_LoseScreenName = "GameLoseScreen";

        [Header("Blur")]
        [SerializeField] Volume m_Volume;

        // string IDs
        const string k_PauseButton = "pause__button";

        const string k_CharPanel = "game-char__panel";
        const string k_FrameFX = "game-char__fx-frame";

        const string k_MusicSlider = "pause-screen-music-slider";
        const string k_SfxSlider = "pause-screen-sfx-slider";

        const string k_ResumeButton = "pause-screen__resume-button";
        const string k_PauseQuitButton = "pause-screen__quit-button";
        const string k_BackButton = "pause-screen__back-button";

        const string k_WinNextButton = "game-win__next-button";
        const string k_LoseQuitButton = "game-lose__quit-button";
        const string k_LoseRetryButton = "game-lose__retry-button";

        const float k_DelayWinScreen = 2f;

        // string IDs
        // references to functional UI elements (buttons and screens)
        VisualElement m_PauseScreen;
        VisualElement m_WinScreen;
        VisualElement m_LoseScreen;
        VisualElement m_CharPortraitContainer;

        Slider m_MusicSlider;
        Slider m_SfxSlider;

        Button m_PauseButton;
        Button m_PauseResumeButton;
        Button m_PauseQuitButton;
        Button m_PauseBackButton;

        Button m_WinNextButton;
        Button m_LoseQuitButton;
        Button m_LoseRetryButton;

        UIDocument m_GameScreen;

        bool m_IsGameOver;

        void OnEnable()
        {
            SetVisualElements();
            RegisterButtonCallbacks();

            if (m_Volume == null)
                m_Volume = FindObjectOfType<Volume>();

            GameScreenController.GameWon += OnGameWon;
            GameScreenController.GameLost += OnGameLost;
            GameScreenController.HideCharacterCard += OnHideCharacterCard;
            GameScreenController.SettingsUpdated += OnSettingsUpdated;

            UnitController.SpecialCharged += OnSpecialCharged;
            UnitController.SpecialDischarged += OnSpecialDischarged;
        }

 

        void OnDisable()
        {
            GameScreenController.GameWon -= OnGameWon;
            GameScreenController.GameLost -= OnGameLost;
            GameScreenController.HideCharacterCard -= OnHideCharacterCard;
            GameScreenController.SettingsUpdated -= OnSettingsUpdated;

            UnitController.SpecialCharged -= OnSpecialCharged;
            UnitController.SpecialDischarged -= OnSpecialDischarged;
        }

        void SetVisualElements()
        {
            m_GameScreen = GetComponent<UIDocument>();
            VisualElement rootElement = m_GameScreen.rootVisualElement;

            m_PauseScreen = rootElement.Q(m_PauseScreenName);
            m_WinScreen = rootElement.Q(m_WinScreenName);
            m_LoseScreen = rootElement.Q(m_LoseScreenName);

            m_PauseButton = rootElement.Q<Button>(k_PauseButton);
            m_PauseResumeButton = rootElement.Q<Button>(k_ResumeButton);
            m_PauseQuitButton = rootElement.Q<Button>(k_PauseQuitButton);
            m_PauseBackButton = rootElement.Q<Button>(k_BackButton);

            m_WinNextButton = rootElement.Q<Button>(k_WinNextButton);
            m_LoseQuitButton = rootElement.Q<Button>(k_LoseQuitButton);
            m_LoseRetryButton = rootElement.Q<Button>(k_LoseRetryButton);
            m_CharPortraitContainer = rootElement.Q<VisualElement>(k_CharPanel);

            m_MusicSlider = rootElement.Q<Slider>(k_MusicSlider);
            m_SfxSlider = rootElement.Q<Slider>(k_SfxSlider);
        }

        void RegisterButtonCallbacks()
        {
            // set up buttons with RegisterCallback
            m_PauseButton?.RegisterCallback<ClickEvent>(ShowPauseScreen);
            m_PauseResumeButton?.RegisterCallback<ClickEvent>(ResumeGame);
            m_PauseBackButton?.RegisterCallback<ClickEvent>(ResumeGame);
            m_PauseQuitButton?.RegisterCallback<ClickEvent>(QuitGame);

            m_WinNextButton?.RegisterCallback<ClickEvent>(QuitGame);
            m_LoseQuitButton?.RegisterCallback<ClickEvent>(QuitGame);
            m_LoseRetryButton?.RegisterCallback<ClickEvent>(RestartGame);

            m_MusicSlider?.RegisterValueChangedCallback(ChangeMusicVolume);
            m_SfxSlider?.RegisterValueChangedCallback(ChangeSfxVolume);
        }

        void Start()
        {
            BlurBackground(false);
        }

        void ShowVisualElement(VisualElement visualElement, bool state)
        {
            if (visualElement == null)
                return;

            visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
        }

        // add the character portrait to the container
        public void AddHero(CharacterCard card)
        {
            if (m_CharPortraitContainer == null)
            {
                SetVisualElements();
            }

            m_CharPortraitContainer.Add(card.CharacterTemplate);
            card.CharacterTemplate.pickingMode = PickingMode.Ignore;
            EnableFrameFX(card.CharacterTemplate, false);
        }

        void ShowPauseScreen(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();

            GamePaused?.Invoke(1f);
            
            ShowVisualElement(m_PauseScreen, true);
            ShowVisualElement(m_PauseButton, false);

            BlurBackground(true);

            m_CharPortraitContainer.style.display = DisplayStyle.None;
        }

        void RestartGame(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            GameRestarted?.Invoke();
        }
        void QuitGame(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            GameQuit?.Invoke();
        }

        void ResumeGame(ClickEvent evt)
        {
            GameResumed?.Invoke();
            AudioManager.PlayDefaultButtonSound();
            ShowVisualElement(m_PauseScreen, false);
            ShowVisualElement(m_PauseButton, true);
            BlurBackground(false);

            m_CharPortraitContainer.style.display = DisplayStyle.Flex;
        }

        // use Volume to blur the background GameObjects
        void BlurBackground(bool state)
        {
            if (m_Volume == null)
                return;

            DepthOfField blurDOF;
            if (m_Volume.profile.TryGet<DepthOfField>(out blurDOF))
            {
                blurDOF.active = state;
            }
        }

        //  disable the CharacterCard Visual Element
        void HideCharacterCard(UnitController heroUnit)
        {
            // enemy does have CharacterCard
            if (heroUnit.CharacterCard == null)
                return;

            if (m_CharPortraitContainer == null)
                return;

            // disable character card
            VisualElement charCard = GetCharacterCard(heroUnit);
            charCard.style.display = DisplayStyle.None;
        }

        // find the matching CharacterCard
        VisualElement GetCharacterCard(UnitController heroUnit)
        {
            // the hero unit's template Visual Tree Asset
            TemplateContainer cardTemplate = heroUnit.CharacterCard.CharacterTemplate;

            // all character portrait cards
            List<VisualElement> cardElements = m_CharPortraitContainer.Children().ToList();

            // return the match
            foreach (VisualElement card in cardElements)
            {
                if (card == cardTemplate)
                {
                    return card;
                }
            }
            return null;
        }

        // frame fx for special abilities
        void EnableFrameFX(VisualElement card, bool state)
        {
            if (card == null)
                return;

            VisualElement frameFx = card.Q<VisualElement>(k_FrameFX);
            ShowVisualElement(frameFx, state);
        }

        IEnumerator GameLostRoutine()
        {
            // wait, then show lose screen and blur bg
            yield return new WaitForSeconds(k_DelayWinScreen);

            // hide UI
            m_CharPortraitContainer.style.display = DisplayStyle.None;
            m_PauseButton.style.display = DisplayStyle.None;

            AudioManager.PlayDefeatSound();
            ShowVisualElement(m_LoseScreen, true);
            BlurBackground(true);
        }

        IEnumerator GameWonRoutine()
        {
            Time.timeScale = 0.5f;
            yield return new WaitForSeconds(k_DelayWinScreen);

            // hide the UI
            m_CharPortraitContainer.style.display = DisplayStyle.None;
            m_PauseButton.style.display = DisplayStyle.None;

            AudioManager.PlayVictorySound();
            ShowVisualElement(m_WinScreen, true);
        }

        // volume settings
        void ChangeSfxVolume(ChangeEvent<float> evt)
        {
            MusicVolumeChanged?.Invoke(evt.newValue);
        }

        void ChangeMusicVolume(ChangeEvent<float> evt)
        {
            SfxVolumeChanged?.Invoke(evt.newValue);
        }

        // event-handling methods
        void OnGameWon()
        {
            if (m_IsGameOver)
                return;

            m_IsGameOver = true;
            StartCoroutine(GameWonRoutine());
        }

        void OnGameLost()
        {
            if (m_IsGameOver)
                return;

            m_IsGameOver = true;
            StartCoroutine(GameLostRoutine());
        }

        void OnHideCharacterCard(UnitController unit)
        {
            HideCharacterCard(unit);
        }

        private void OnSpecialDischarged(UnitController unit)
        {
            VisualElement card = GetCharacterCard(unit);
            EnableFrameFX(card, false);
        }

        private void OnSpecialCharged(UnitController unit)
        {
            VisualElement card = GetCharacterCard(unit);
            EnableFrameFX(card, true);
        }

        void OnSettingsUpdated(GameData gameData)
        {
            m_MusicSlider.value = gameData.musicVolume;
            m_SfxSlider.value = gameData.sfxVolume;
        }
    }
}
