using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace UIToolkitDemo
{
    // non-UI logic for HomeScreen
    public class HomeScreenController : MonoBehaviour
    {
        // events
        public static event Action<LevelSO> ShowLevelInfo;
        public static event Action<List<ChatSO>> ShowChats;
        public static event Action MainMenuExited;

        [Header("Level Data")]
        [SerializeField] LevelSO m_LevelData;

        [Header("Chat Data")]
        [SerializeField] string m_ChatResourcePath = "GameData/Chat";

        // chat messages to display
        [SerializeField] List<ChatSO> m_ChatData;

        void Awake()
        {
            m_ChatData?.AddRange(Resources.LoadAll<ChatSO>(m_ChatResourcePath));
        }

        void OnEnable()
        {
            HomeScreen.PlayButtonClicked += OnPlayGameLevel;
        }

        void OnDisable()
        {
            HomeScreen.PlayButtonClicked -= OnPlayGameLevel;
        }

        void Start()
        {
            ShowLevelInfo?.Invoke(m_LevelData);
            ShowChats?.Invoke(m_ChatData);
        }

        // scene-management methods
        public void OnPlayGameLevel()
        {
            if (m_LevelData == null)
                return;

            MainMenuExited?.Invoke();

#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                SceneManager.LoadSceneAsync(m_LevelData.sceneName);
        }
    }
}
