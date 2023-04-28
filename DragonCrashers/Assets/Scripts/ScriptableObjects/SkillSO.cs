using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIToolkitDemo
{
    public enum SkillCategory
    {
        Basic = 0,
        Intermediate = 1,
        Advanced = 2
    }

    // each character has up to three special attack skills
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/Skills/SkillGameData", menuName = "UIToolkitDemo/Skill", order = 3)]
    public class SkillSO : ScriptableObject
    {
        const int levelsPerTier = 5;

        public string skillName;

        // tier: 0 = "basic", 1 = unlocked at level 3, 2 = unlocked at level 6
        public SkillCategory category;

        // damage applied over damage time
        public int damagePoints;

        // time in seconds
        public float damageTime;

        // icon for character screen
        public Sprite sprite;

        public int MinLevel
        {
            get
            {
                switch (category)
                {
                    case SkillCategory.Basic:
                        return 0;
                    case SkillCategory.Intermediate:
                        return 3;
                    case SkillCategory.Advanced:
                        return 6;
                    default:
                        return 0;
                }
            }
        }

        public string GetCategoryText()
        {
            return (SkillCategory)category + " attack";
        }

        public string GetTierText(int CurrentLevel)
        {
            int currentTier = (int) Mathf.Floor(CurrentLevel / levelsPerTier) + 1;
            return "Tier " + currentTier;
        }

        public string GetDamageText()
        {
            return "Deals <color=#775027>" + damagePoints + " Damage</color> points to an enemy every <color=#775027>" + damageTime + " seconds</color>";
        }

        public string GetNextTierLevelText(int currentLevel)
        {
            int currentTier = (int)Mathf.Floor(currentLevel / levelsPerTier) + 1;
            int nextLevel = currentTier  * levelsPerTier;
            return "Next tier at Level " + nextLevel;
        }

        public bool IsUnlocked(int currentLevel)
        {
            return currentLevel > MinLevel;
        }

        public string GetLockText()
        {
            return "Unlocked at Level " + MinLevel;
        }
    }
}