using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

namespace UIToolkitDemo
{
    public class HomeScreen : MenuScreen
    {
        public static event Action PlayButtonClicked;
        public static event Action HomeScreenShown;

        const string k_LevelNumberName = "home-play__level-number";
        const string k_PlayLevelButtonName = "home-play__level-button";

        const string k_LevelLabelName = "home-play__level-name";
        const string k_LevelThumbnailName = "home-play__level-panel";

        VisualElement m_PlayLevelButton;
        VisualElement m_LevelThumbnail;

        Label m_LevelNumber;
        Label m_LevelLabel;

        void OnEnable()
        {
            HomeScreenController.ShowLevelInfo += OnShowLevelInfo;
        }

        void OnDisable()
        {
            HomeScreenController.ShowLevelInfo -= OnShowLevelInfo;
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            m_PlayLevelButton = m_Root.Q(k_PlayLevelButtonName);
            m_LevelLabel = m_Root.Q<Label>(k_LevelLabelName);
            m_LevelNumber = m_Root.Q<Label>(k_LevelNumberName);

            m_LevelThumbnail = m_Root.Q(k_LevelThumbnailName);
        }

        protected override void RegisterButtonCallbacks()
        {
            m_PlayLevelButton?.RegisterCallback<ClickEvent>(ClickPlayButton);
        }

        private void ClickPlayButton(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            PlayButtonClicked?.Invoke();
        }

        public override void ShowScreen()
        {
            base.ShowScreen();
            HomeScreenShown?.Invoke();
        }

        // shows the level information
        public void ShowLevelInfo(int levelNumber, string levelName, Sprite thumbnail)
        {
            m_LevelNumber.text = "Level " + levelNumber;
            m_LevelLabel.text = levelName;
            m_LevelThumbnail.style.backgroundImage = new StyleBackground(thumbnail);
        }

        // event-handling methods

        void OnShowLevelInfo(LevelSO levelData)
        {
            if (levelData == null)
                return;

            ShowLevelInfo(levelData.levelNumber, levelData.levelLabel, levelData.thumbnail);
        }
    }
}