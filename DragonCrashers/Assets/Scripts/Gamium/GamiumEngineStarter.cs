using System.Collections.Generic;
using Gamium;
using UnityEngine;

public class GamiumEngineStarter
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
                    new InputMapping()
                    {
                        alias = "Horizontal", positiveCodes = new HashSet<KeyCode>() { KeyCode.D },
                        negativeCodes = new HashSet<KeyCode>() { KeyCode.A }
                    },
                    new InputMapping()
                    {
                        alias = "Vertical", positiveCodes = new HashSet<KeyCode>() { KeyCode.W },
                        negativeCodes = new HashSet<KeyCode>() { KeyCode.S }
                    },
                },
                isVerbose = true
            }).Run();
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
    }
}