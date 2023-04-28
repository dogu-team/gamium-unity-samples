using UnityEngine;

namespace Util
{
    public class SingletonMonoBehavior<T> : MonoBehaviour where T : SingletonMonoBehavior<T>, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (null == instance)
                {
                    var go = new GameObject(typeof(T).ToString());
                    instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }

                return instance;
            }
        }
    }
}