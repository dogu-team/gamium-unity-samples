using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    public class ScreenFader : MenuScreen
    {

        //const string m_ScreenFaderName = "main-menu-screen-fader";
        //const string m_ScreenFaderOnClassName = "screen__fader--on";
        //const string m_ScreenFaderOffClassName = "screen__fader--off";

        //public void FadeScreen(bool state)
        //{
        //    if (m_ScreenFader == null)
        //        return;

        //    StartCoroutine(FadeScreenRoutine(state));
        //}

        //IEnumerator FadeScreenRoutine(bool state, float delay = 0.25f)
        //{
        //    // short delay to allow the UI Elements to set up
        //    yield return new WaitForSeconds(delay);

        //    if (state)
        //    {
        //        m_ScreenFader?.RemoveFromClassList(m_ScreenFaderOffClassName);
        //        m_ScreenFader?.AddToClassList(m_ScreenFaderOnClassName);
        //    }
        //    else
        //    {
        //        m_ScreenFader?.RemoveFromClassList(m_ScreenFaderOnClassName);
        //        m_ScreenFader?.AddToClassList(m_ScreenFaderOffClassName);
        //    }
        //}

        void RegisterCallbacks()
        {
            // waits for interface to build (GeometryChangedEvent) and then fades off the ScreenFader
            //m_ScreenFader?.RegisterCallback<GeometryChangedEvent>(evt =>
            //{
            //    FadeScreen(false);
            //});
        }

    }
}
