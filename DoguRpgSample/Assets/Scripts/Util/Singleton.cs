namespace Util
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new T();
                    instance.InitSingleton();
                }

                return instance;
            }
        }

        protected virtual void InitSingleton()
        {
        }
    }
}