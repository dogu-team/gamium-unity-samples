﻿using System.Collections.Generic;
using Dypsloom.DypThePenguin.Scripts.Character;
using Gamium;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerStarter
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnLoad()
    {
        new ServerBuilder()
            .SetStateHandler(new EventHandler())
            .SetConfig(new ServerConfig
            {
                inputMappings = new InputMapping[]
            {
                new InputMapping(){alias = "Horizontal"
                    , positiveCodes = new HashSet<KeyCode>() {KeyCode.D}, negativeCodes = new HashSet<KeyCode>() {KeyCode.A}}
                , new InputMapping(){alias = "Vertical"
                    , positiveCodes = new HashSet<KeyCode>() {KeyCode.W}, negativeCodes = new HashSet<KeyCode>() {KeyCode.S}}
                , new InputMapping(){alias = "Jump", positiveCodes = new HashSet<KeyCode>() {KeyCode.Space}}
                , new InputMapping(){alias = "Fire1", positiveCodes = new HashSet<KeyCode>() {KeyCode.LeftControl}}
                , new InputMapping(){alias = "Fire2", positiveCodes = new HashSet<KeyCode>() {KeyCode.LeftAlt}}
            },
                isVerbose = true
            }).StartServer();
    }

    private class EventHandler : Gamium.IEventHandler
    {
        private bool isInit;
        public void OnAccept(EventContext eventContext)
        {
        }

        public void Update()
        {
        }

        public void OnClose(EventContext eventContext)
        {
        }

        private Character GetCharacter()
        {
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in roots)
            {
                var character = root.GetComponentInChildren<Character>();
                if (null != character) return character;
            }

            return null;
        }
    }
}