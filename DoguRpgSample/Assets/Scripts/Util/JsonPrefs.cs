using Newtonsoft.Json;
using UnityEngine;

namespace Util
{
    public static class JsonPrefs
    {
        public static void Save<T>(string key, T obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            // string json = JsonUtility.ToJson(obj);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static T Load<T>(string key)
        {
            string json = PlayerPrefs.GetString(key);
            return JsonConvert.DeserializeObject<T>(json);
            // return JsonUtility.FromJson<T>(json);
        }
    }
}