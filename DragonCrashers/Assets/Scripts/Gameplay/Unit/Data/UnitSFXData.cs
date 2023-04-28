using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    [CreateAssetMenu(fileName = "Data_SFX_", menuName = "Dragon Crashers/Unit/SFX Data", order = 1)]
    public class UnitSFXData : ScriptableObject
    {

        [Header("Battle SFX")]
        public AudioClip[] audioClipsGetHit;
        public AudioClip[] audioClipsDeath;

        public AudioClip GetGetHitClip()
        {
            return SelectRandomAudioClip(audioClipsGetHit);
        }

        public AudioClip GetDeathClip()
        {
            return SelectRandomAudioClip(audioClipsDeath);
        }

        AudioClip SelectRandomAudioClip(AudioClip[] audioClipArray)
        {

            if(audioClipArray.Length <= 0)
            {
                return null;
            }

            int randomClipInt = Random.Range(0, audioClipArray.Length);
            return audioClipArray[randomClipInt];
        }

    }

