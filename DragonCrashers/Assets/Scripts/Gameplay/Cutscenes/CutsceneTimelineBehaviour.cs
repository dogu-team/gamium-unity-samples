using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class CutsceneTimelineBehaviour : MonoBehaviour
{

    [Header("Timeline")]
    public PlayableDirector cutsceneTimeline;

    [Header("Marker Events")]
    public UnityEvent cutsceneTimelineFinished;

    public void StartTimeline()
    {
        cutsceneTimeline.Play();
    }

    public void TimelineFinished()
    {
        cutsceneTimelineFinished.Invoke();
    }
}
