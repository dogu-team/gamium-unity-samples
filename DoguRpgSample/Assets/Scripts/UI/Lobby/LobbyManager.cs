using System;
using UnityEngine;

namespace UI.Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        public LobbyCreateCharacter createCharacter;
        public LobbySelectCharacter selectCharacter;

        public void Awake()
        {
            createCharacter.gameObject.SetActive(false);
            createCharacter.onCreateCharacterDone = OnCreateCharacterDone;
            selectCharacter.gameObject.SetActive(true);
            selectCharacter.onCreateCharacterClicked = OnCreateCharacterStart;
        }

        public void OnCreateCharacterStart()
        {
            createCharacter.gameObject.SetActive(true);
            selectCharacter.gameObject.SetActive(false);
        }

        public void OnCreateCharacterDone()
        {
            createCharacter.gameObject.SetActive(false);
            selectCharacter.gameObject.SetActive(true);
            selectCharacter.Refresh();
        }
    }
}