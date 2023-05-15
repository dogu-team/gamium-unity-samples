using System.Collections.Generic;
using Gamium;
using UnityEngine;

public class GamiumEngineStarter
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnLoad()
    {
        new ServerBuilder().Run();
    }
}