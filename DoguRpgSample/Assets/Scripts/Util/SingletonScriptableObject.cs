using System;
using UnityEngine;

namespace Util
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (null == instance)
                {
                    T defaultT = ScriptableObject.CreateInstance<T>();
                    if (null == defaultT)
                    {
                        Debug.LogError($"CreateInstance {typeof(T)} failed");
                        return null;
                    }
                    T newT = Resources.Load<T>(defaultT.GetResourcePath());
                    if (null == newT)
                    {
                        Debug.LogError($"Load {typeof(T)} from {defaultT.GetResourcePath()} failed");
                        return null;
                    }
                    Debug.Log($"Load {typeof(T)} from {newT.GetResourcePath()}");
                    newT.OnInit();

                    instance = newT;
                }

                return instance;
            }
        }

        protected abstract void OnInit();
        public abstract string GetResourcePath();
    }
}