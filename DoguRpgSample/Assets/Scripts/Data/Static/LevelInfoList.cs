using UnityEditor;
using UnityEngine;
using Util;

namespace Data.Static
{
    public class LevelInfoList : SingletonScriptableObject<LevelInfoList>
    {
        public ChartValue expChart;
        private const string RESOURCE_PATH = "ScriptableObjects/Data/Level";
        private const string TYPE_NAME = "LevelInfoList";


        protected override void OnInit()
        {
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!EditorConditions.IsValidatable()) return;
#endif
            ChartValue.Validate(expChart);
        }

        public void GetLevelInfoFromExp(long exp, out LevelInfo currentLevelInfo, out LevelInfo nextLevelInfo)
        {
            var points = expChart.GetPoints();
            currentLevelInfo = new LevelInfo
            {
                level = 1,
                miniumExperience = points[0]
            };
            nextLevelInfo = new LevelInfo
            {
                level = 2,
                miniumExperience = points[1]
            };

            for (int i = 1; i < points.Count; i++)
            {
                var point = points[i];
                if (point > exp)
                {
                    currentLevelInfo = new LevelInfo
                    {
                        level = i,
                        miniumExperience = points[i - 1]
                    };
                    nextLevelInfo = new LevelInfo
                    {
                        level = i + 1,
                        miniumExperience = points[i]
                    };
                    return;
                }
            }
        }

        public float GetExpPercent(long exp, LevelInfo currentLevelInfo, LevelInfo nextLevelInfo)
        {
            return (100 * (exp - currentLevelInfo.miniumExperience)) /
                   (nextLevelInfo.miniumExperience - currentLevelInfo.miniumExperience);
        }

        public override string GetResourcePath()
        {
            return $"{RESOURCE_PATH}/{TYPE_NAME}";
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/" + RESOURCE_PATH + "/" + TYPE_NAME)]
        static void CreateAsset()
        {
            AssetDatabaseUtil.CheckAndMakeDirectory($"Assets/Resources/{RESOURCE_PATH}");
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<LevelInfoList>(),
                $"Assets/Resources/{RESOURCE_PATH}/{TYPE_NAME}.asset");
        }
#endif
    }
}