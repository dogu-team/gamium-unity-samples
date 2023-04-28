using System;
using Data;
using Data.Static;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI.Lobby
{
    public class LobbyCreateCharacter : MonoBehaviour
    {
        public LayoutGroup characterList;
        public Text characterDescription;
        public InputField nicknameInputField;
        public Text nicknameWarningText;
        public Action onCreateCharacterDone;
        public GameObject playerSpawnPoint;

        private Data.Static.CharacterInfo currentCharacterInfo;

        public void Awake()
        {
            var playerCharacters = CharacterInfoList.Instance.GetEntries(CharacterType.Player);
            for (int i = 0; i < playerCharacters.Length; i++)
            {
                var buttonPrefab = UIPrefabs.Instance.squareButtonPrefab;
                var characterInfo = playerCharacters[i];
                var button = Instantiate(buttonPrefab);
                button.transform.SetParent(characterList.transform, false);
                button.GetComponentInChildren<Text>().text = characterInfo.value.nickname;

                button.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(characterInfo));
            }

            OnCharacterSelected(playerCharacters[0]);
        }

        public void OnCharacterSelected(Data.Static.CharacterInfo characterInfo)
        {
            currentCharacterInfo = characterInfo;
            characterDescription.text = characterInfo.value.description;

            SpawnCharacter(currentCharacterInfo);
        }

        public void OnOkClicked()
        {
            // check if username has space
            if (nicknameInputField.text.Contains(" "))
            {
                nicknameWarningText.text = "Character name cannot contain space";
                return;
            }

            // check if username is empty
            if (nicknameInputField.text == string.Empty)
            {
                nicknameWarningText.text = "Character name cannot be empty";
                return;
            }

            var newPlayerCharacter = new PlayerCharacterData
            {
                characterId = currentCharacterInfo.value.id,
                nickname = nicknameInputField.text
            };
            PlayerDataController.Instance.AddPlayerCharacter(newPlayerCharacter);
            onCreateCharacterDone?.Invoke();
        }

        private void SpawnCharacter(Data.Static.CharacterInfo characterInfo)
        {
            TransformUtil.DestroyChildren(playerSpawnPoint.transform);
            Instantiate(characterInfo.value.prefab, playerSpawnPoint.transform, false);
        }
    }
}