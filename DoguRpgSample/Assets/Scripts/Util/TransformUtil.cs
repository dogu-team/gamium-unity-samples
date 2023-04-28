using UnityEngine;

namespace Util
{
    public static class TransformUtil
    {
        public static void DestroyChildren(Transform t)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                GameObject.Destroy(t.GetChild(i).gameObject);
            }
        }
    }
}