namespace Util
{
    internal static class VectorExtensions
    {
        internal static UnityEngine.Vector3 XZPlane(this UnityEngine.Vector3 v)
        {
            return new UnityEngine.Vector3(v.x, 0, v.z);
        }
    }
}