using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class UnitCharacterAnimationBehaviour : MonoBehaviour
    {
        [Header("Animator")]
        public Animator characterAnimator;

        private string animGetHitParameter = "Get Hit";
        private int animGetHitID;

        private string animDieParameter = "Die";
        private int animDieID;

        void Awake()
        {
            SetupAnimationIDs();
        }

        void SetupAnimationIDs()
        {
            animGetHitID = Animator.StringToHash(animGetHitParameter);
            animDieID = Animator.StringToHash(animDieParameter);
        }

        public void CharacterWasHit()
        {
            characterAnimator.SetTrigger(animGetHitID);
        }

        public void CharacterHasDied()
        {
            characterAnimator.SetTrigger(animDieID);
        }
    }

