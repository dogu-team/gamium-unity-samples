using UnityEditor;

#if UNITY_EDITOR
namespace Util
{
    public static class AssetDatabaseUtil
    {
        public static void CheckAndMakeDirectory(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                var splited = path.Split('/');
                var sum = "Assets";
                for (int i = 1; i < splited.Length; i++)
                {
                    var cur = splited[i];
                    if (!AssetDatabase.IsValidFolder(sum + "/" + cur))
                        AssetDatabase.CreateFolder(sum, cur);
                    sum += "/" + cur;
                }
            }
        }
    }
}
#endif