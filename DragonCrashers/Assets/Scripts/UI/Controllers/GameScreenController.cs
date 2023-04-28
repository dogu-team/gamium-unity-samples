using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UIToolkitDemo
{
    // non-UI logic for the GameScreen
    public class GameScreenController : MonoBehaviour
    {
        public static event Action GameWon;
        public static event Action GameLost;
        public static event Action<UnitController> HideCharacterCard;

        public static event Action<GameData> SettingsUpdated;
        public static event Action SettingsLoad;

        [Header("Scenes")]
        [SerializeField] string m_MainMenuSceneName = "MainMenu";
        [SerializeField] string m_GameSceneName = "Game";

        // temp storage to send back to GameDataManager
        GameData m_SettingsData;

        void OnEnable()
        {
            BattleGameplayManager.GameWon += OnGameWon;
            BattleGameplayManager.GameLost += OnGameLost;

            GameScreen.GamePaused += OnGamePaused;
            GameScreen.GameResumed += OnGameResumed;
            GameScreen.GameQuit += OnGameQuit;
            GameScreen.GameRestarted += OnGameRestarted;
            GameScreen.MusicVolumeChanged += OnMusicVolumeChanged;
            GameScreen.SfxVolumeChanged += OnSfxVolumeChanged;

            UnitController.UnitDied += OnUnitDied;

            SaveManager.GameDataLoaded += OnGameDataLoaded; 
        }

        void OnDisable()
        {
            BattleGameplayManager.GameWon -= OnGameWon;
            BattleGameplayManager.GameLost -= OnGameLost;

            GameScreen.GamePaused -= OnGamePaused;
            GameScreen.GameResumed -= OnGameResumed;
            GameScreen.GameQuit -= OnGameQuit;
            GameScreen.GameRestarted -= OnGameRestarted;

            UnitController.UnitDied -= OnUnitDied;

            SaveManager.GameDataLoaded -= OnGameDataLoaded;
        }

        IEnumerator PauseGameTime(float delay = 2f)
        {

            float pauseTime = Time.time + delay;
            float decrement = (delay > 0) ? Time.deltaTime / delay : Time.deltaTime;

            while (Time.timeScale > 0.1f || Time.time < pauseTime)
            {
                Time.timeScale = Mathf.Clamp(Time.timeScale - decrement, 0f, Time.timeScale - decrement);
                yield return null;
            }

            // ramp the timeScale down to 0
            Time.timeScale = 0f;
        }

        // scene-management methods
        void QuitGame()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                SceneManager.LoadSceneAsync(m_MainMenuSceneName);
        }

        void RestartLevel()
        {

            Time.timeScale = 1f;
#if UNITY_EDITOR
            if (Application.isPlaying)

#endif
                SceneManager.LoadSceneAsync(m_GameSceneName);
        }

        // event-handling methods
        void OnGameLost()
        {
            GameLost?.Invoke();
        }

        void OnGameWon()
        {
            GameWon?.Invoke();
        }

        void OnGamePaused(float delay)
        {
            SettingsLoad?.Invoke();
            StopAllCoroutines();
            StartCoroutine(PauseGameTime(delay));
        }

        void OnGameResumed()
        {
            SettingsUpdated?.Invoke(m_SettingsData);
            StopAllCoroutines();
            Time.timeScale = 1f;
        }

        void OnGameRestarted()
        {
            RestartLevel();
        }

        void OnGameQuit()
        {
            QuitGame();
        }

        void OnUnitDied(UnitController deadHero)
        {
            HideCharacterCard?.Invoke(deadHero);
        }

        void OnSfxVolumeChanged(float sfxVolume)
        {
            m_SettingsData.musicVolume = sfxVolume;

            SettingsUpdated?.Invoke(m_SettingsData);
        }

        void OnMusicVolumeChanged(float musicVolume)
        {
            m_SettingsData.sfxVolume = musicVolume;

            SettingsUpdated?.Invoke(m_SettingsData);
        }

        void OnGameDataLoaded(GameData gameData)
        {
            if (gameData == null)
                return;

            m_SettingsData = gameData;

            m_SettingsData.musicVolume = gameData.musicVolume;
            m_SettingsData.sfxVolume = gameData.sfxVolume;

            SettingsUpdated?.Invoke(gameData);

        }
    }
}