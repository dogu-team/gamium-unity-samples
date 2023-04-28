namespace Util
{
    public static class Interpolation
    {
        public static float Linear(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static float Exponential(float a, float b, float t)
        {
            return a + (b - a) * t * t;
        }
    }
}