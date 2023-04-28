using UnityEngine;
using Utilities.Inspector;
using UnityEngine.SceneManagement;

public class NextSceneLoader
{
    public void LoadNextScene(SceneField sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}