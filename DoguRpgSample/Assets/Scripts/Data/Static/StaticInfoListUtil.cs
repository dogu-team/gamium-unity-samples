using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Util;

namespace Data.Static
{
    public static class StaticInfoListUtil
    {
        public static T[] LoadAssetsAndSort<T>(string path, Func<T, int> getIdFunc) where T : ScriptableObject
        {
            if (!EditorConditions.IsValidatable())
            {
                Debug.Log("StaticInfoListUtil.LoadAssetsAndSort Return Empty");
                return new T[]{};
            }
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}",
                new[] { path });
            var infos = guids.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>)
                .ToArray();
            Array.Sort(infos, (a, b) => { return getIdFunc.Invoke(a) - getIdFunc.Invoke(b); });

            var dict = new Dictionary<int, T>();
            foreach (var info in infos)
            {
                if (dict.ContainsKey(getIdFunc(info)))
                {
                    Debug.LogError(
                        $"{typeof(T).Name}List has duplicate id: {info.name} with {dict[getIdFunc(info)].name}");
                }
                else
                {
                    dict.Add(getIdFunc(info), info);
                }
            }

            return infos;
#else
            return new T[]{};
#endif
        }
    }
}