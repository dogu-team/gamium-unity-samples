using UnityEditor;

namespace Util
{
    public static class EditorConditions
    {
        public static bool IsValidatable()
        {
#if UNITY_EDITOR
            if (BuildPipeline.isBuildingPlayer || EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling) return false;
            return true;
#else
            return false;
#endif

        }
        
    }
}