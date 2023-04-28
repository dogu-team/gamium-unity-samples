
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Inspector;
using UnityEngine.SceneManagement;

public class LoadSceneBehaviour : MonoBehaviour
{

    public SceneField sceneToLoad; 

    public void LoadScene()
    {
        NextSceneLoader sceneLoader = new NextSceneLoader();
        sceneLoader.LoadNextScene(sceneToLoad);
    }
    
}
