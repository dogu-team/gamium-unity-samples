using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace UIToolkitDemo
{
    // holds basic level information (label name, level number, scene name for loading, thumbnail graphic for display, etc.)
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/Levels/LevelData", menuName = "UIToolkitDemo/Level", order = 11)]
    public class LevelSO : ScriptableObject
    {
        public int levelNumber;
        public string levelLabel;
        public Sprite thumbnail;
        public string sceneName;
    }
}
