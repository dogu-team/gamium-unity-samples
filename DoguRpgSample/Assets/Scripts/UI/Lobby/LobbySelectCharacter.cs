using System;
using System.Linq;
using Data.Static;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;
using CharacterInfo = Data.Static.CharacterInfo;

namespace UI.Lobby
{
    public class LobbySelectCharacter : MonoBehaviour
    {
        public LayoutGroup characterList;
        public Text characterDescription;
        public Action onCreateCharacterClicked;
        public GameObject playerSpawnPoint;
        private int currentCharacterIndex = 0;

        void Awake()
        {
            Refresh();
        }

        public void Refresh()
        {
            TransformUtil.DestroyChildren(characterList.transform);

            var buttonPrefab = UIPrefabs.Instance.squareButtonPrefab;
            for (int i = 0; i < PlayerDataController.Instance.Characters.Count; i++)
            {
                var characterInfo = PlayerDataController.Instance.Characters[i];
                var button = Instantiate(buttonPrefab);
                button.transform.SetParent(characterList.transform, false);
                button.GetComponentInChildren<Text>().text = characterInfo.nickname;

                var iCopied = i;
                button.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(iCopied));
            }

            var addButton = Instantiate(buttonPrefab);
            addButton.transform.SetParent(characterList.transform, false);
            var addButtonText = addButton.GetComponentInChildren<Text>();
            addButtonText.text = "+";
            addButtonText.resizeTextForBestFit = true;
            addButton.GetComponent<Button>().onClick.AddListener(() => onCreateCharacterClicked?.Invoke());

            OnCharacterSelected(0);
        }

        void OnCharacterSelected(int index)
        {
            if (PlayerDataController.Instance.Characters.Count <= index)
            {
                characterDescription.text = "Please create a character";
                return;
            }

            var playerCharacter = PlayerDataController.Instance.Characters[index];
            var info = CharacterInfoList.Instance.GetEntry(playerCharacter.characterId);
            LevelInfoList.Instance.GetLevelInfoFromExp(playerCharacter.Experience, out var levelInfo,
                out var nextLevelInfo);

            characterDescription.text = $"level {levelInfo.level}";
            SpawnCharacter(info);
            currentCharacterIndex = index;
        }

        public void OnDeleteCharacterClicked()
        {
            if (!PlayerDataController.Instance.Characters.Any())
            {
                return;
            }

            var playerCharacter = PlayerDataController.Instance.Characters[currentCharacterIndex];
            PlayerDataController.Instance.RemovePlayerCharacter(playerCharacter.uuid);
            Refresh();
        }

        public void OnStartClicked()
        {
            if (!PlayerDataController.Instance.Characters.Any())
            {
                return;
            }

            var playerCharacter = PlayerDataController.Instance.Characters[currentCharacterIndex];
            PlayerDataController.Instance.SetCurrentPlayerCharacter(playerCharacter.uuid);
            SceneManager.LoadScene("Game");
        }

        private void SpawnCharacter(CharacterInfo characterInfo)
        {
            TransformUtil.DestroyChildren(playerSpawnPoint.transform);
            Instantiate(characterInfo.value.prefab, playerSpawnPoint.transform, false);
        }
    }
}