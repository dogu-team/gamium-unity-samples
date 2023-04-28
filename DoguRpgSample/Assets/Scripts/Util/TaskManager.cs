using System;
using System.Collections;
using UnityEngine;

namespace Util
{
    public class TaskManager : SingletonMonoBehavior<TaskManager>
    {
        public static void RunCoroutine(IEnumerator routine)
        {
            Instance.StartCoroutine(routine);
        }

        public static void RunNextFrame(Action routine, int frameCount = 1)
        {
            if (0 == frameCount)
            {
                routine?.Invoke();
                return;
            }

            Instance.StartCoroutine(CoRunNextFrame(routine, frameCount));
        }

        public static void RunNextTime(Action routine, float seconds)
        {
            Instance.StartCoroutine(CoRunNextTime(routine, seconds));
        }


        private static IEnumerator CoRunNextFrame(Action action, int frameCount)
        {
            for (int i = 0; i < frameCount; i++)
            {
                yield return null;
            }

            action?.Invoke();
        }

        private static IEnumerator CoRunNextTime(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);

            action?.Invoke();
        }
    }
}