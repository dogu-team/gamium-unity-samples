using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIToolkitDemo;

    public class UnitAudioBehaviour : MonoBehaviour
    {

        

        [Header("Component Reference")]
        public AudioSource audioSource;

        [Header("SFX Volume Override")]
        public float sfxDeathVolume;

     
        void SetAudioSourceVolume(float newVolume)
        {
            audioSource.volume = newVolume;
        }

        void PlayAudioClip(AudioClip selectedAudioClip)
        {
        //audioSource.PlayOneShot(selectedAudioClip);
        AudioManager.PlayOneSFX(selectedAudioClip, Vector3.zero);
        }
    }


