using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using Util;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private string currentAnimationName = string.Empty;
    private List<AnimationWait> animationWaits = new List<AnimationWait>();
    public bool HasWait => animationWaits.Count > 0;

    private class AnimationWait
    {
        public string name;
        public Action completeListener;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];
            clip.AddEvent(new AnimationEvent
            {
                time = 0,
                functionName = "AnimationStartHandler",
                stringParameter = clip.name
            });
            clip.AddEvent(new AnimationEvent
            {
                time = clip.length,
                functionName = "AnimationCompleteHandler",
                stringParameter = clip.name
            });
        }
    }

    public void PlayOnce(string animationName, Action completeListener)
    {
        TaskManager.RunNextFrame(() =>
        {
            animationWaits.Add(new AnimationWait
            {
                name = animationName,
                completeListener = completeListener
            });
            SwitchAnimation(animationName);
            animator.CrossFade(animationName, 0.05f);
        });
    }

    public void PlayOnlyIfNotPlaying(string animationName)
    {
        if (!string.IsNullOrEmpty(currentAnimationName) && currentAnimationName == animationName)
        {
            return;
        }

        if (animator.IsInTransition(0))
        {
            return;
        }

        SwitchAnimation(animationName);
        animator.CrossFade(animationName, 0.3f);
    }

    [Preserve]
    void AnimationStartHandler(string animationName)
    {
    }

    [Preserve]
    void AnimationCompleteHandler(string animationName)
    {
        var waits = animationWaits.Where(a => a.name == animationName);
        foreach (var wait in waits.ToList())
        {
            animationWaits.Remove(wait);
            wait.completeListener?.Invoke();
        }
    }

    void SwitchAnimation(string animationName)
    {
        var beforAnimationName = currentAnimationName;
        currentAnimationName = animationName;
        AnimationCompleteHandler(beforAnimationName);
    }
}