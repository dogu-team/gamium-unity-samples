using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    // manages the Character Stats panel to the right of the CharScreen
    public class CharStatsWindow
    {
        // string IDs
        const string k_LevelLabel = "char-stats__level";

        const string k_CharacterClassIcon = "char-stats__class-icon";
        const string k_CharacterClass = "char-stats__class-label";
        const string k_RarityIcon = "char-stats__rarity-icon";
        const string k_Rarity = "char-stats__rarity-label";
        const string k_AttackTypeIcon = "char-stats__attack-type-icon";
        const string k_AttackType = "char-stats__attack-type-label";

        const string k_BasePointsLife = "char-stats__life-value";
        const string k_BasePointsDefense = "char-stats__defense-value";
        const string k_BasePointsAttack = "char-stats__attack-value";
        const string k_BasePointsAttackSpeed = "char-stats__attack-speed-value";
        const string k_BasePointsSpecialAttack = "char-stats__special-attack-value";
        const string k_BasePointsCriticalHit = "char-stats__critical-hit-value";

        const string k_Skill1Icon = "char-data__skill-icon1";
        const string k_Skill2Icon = "char-data__skill-icon2";
        const string k_Skill3Icon = "char-data__skill-icon3";

        const string k_ActiveFrame = "char-data__skill-active";

        const string k_Skill = "char-data__skill-label";
        const string k_Category = "char-data__skill-category-label";
        const string k_Tier = "char-data__skill-tier-label";
        const string k_Damage = "char-data__skill-tier-damage-label";
        const string k_NextTier = "char-data__skill-next-tier-label";

        const string k_NextSkillButton = "char-data__skill-next-button";
        const string k_LastSkillButton = "char-data__skill-last-button";

        const string k_BioTitle = "char-data__bio-title";
        const string k_BioText = "char-data__bio-text";

        // pairs icons with specific data types
        GameIconsSO m_GameIconsData;

        // inventory, xp, and base data
        CharacterData m_CharacterData;

        // static base data from ScriptableObject
        CharacterBaseSO m_BaseStats;

        // stats tab
        Label m_LevelLabel;

        VisualElement m_CharacterClassIcon;
        Label m_CharacterClass;
        VisualElement m_RarityIcon;
        Label m_Rarity;
        VisualElement m_AttackTypeIcon;
        Label m_AttackType;

        Label m_BasePointsLife;
        Label m_BasePointsDefense;
        Label m_BasePointsAttack;
        Label m_BasePointsAttackSpeed;
        Label m_BasePointsSpecialAttack;
        Label m_BasePointsCriticalHit;

        // skills tab
        VisualElement m_ActiveFrame;
        int m_ActiveIndex;

        VisualElement[] m_SkillIcons = new VisualElement[3];
        SkillSO[] m_BaseSkills = new SkillSO[3];

        Label m_Skill;
        Label m_Category;
        Label m_Tier;
        Label m_Damage;
        Label m_NextTier;

        Button m_NextSkillButton;
        Button m_LastSkillButton;

        // bio tab
        Label m_BioTitle;
        Label m_BioText;

        float timeToNextClick = 0f;
        const float clickCooldown = 0.2f;


        // create a new controller with icon and character data
        public CharStatsWindow(GameIconsSO gameIconData, CharacterData charData)
        {
            m_GameIconsData = gameIconData;
            m_CharacterData = charData;
            m_BaseStats = charData.CharacterBaseData;
        }

        // if the UI already exists, update the character data
        public void UpdateWindow(CharacterData charData, VisualElement panel)
        {
            m_CharacterData = charData;
            m_BaseStats = charData.CharacterBaseData;
            SetGameData();
            ShowSkill(0);
        }

        // query for Visual Elements
        public void SetVisualElements(VisualElement container)
        {
            m_LevelLabel = container.Q<Label>(k_LevelLabel);
            m_CharacterClassIcon = container.Q(k_CharacterClassIcon);
            m_CharacterClass = container.Q<Label>(k_CharacterClass);
            m_RarityIcon = container.Q(k_RarityIcon);
            m_Rarity = container.Q<Label>(k_Rarity);
            m_AttackTypeIcon = container.Q(k_AttackTypeIcon);
            m_AttackType = container.Q<Label>(k_AttackType);

            m_BasePointsLife = container.Q<Label>(k_BasePointsLife);
            m_BasePointsDefense = container.Q<Label>(k_BasePointsDefense);
            m_BasePointsAttack = container.Q<Label>(k_BasePointsAttack);
            m_BasePointsAttackSpeed = container.Q<Label>(k_BasePointsAttackSpeed);
            m_BasePointsSpecialAttack = container.Q<Label>(k_BasePointsSpecialAttack);
            m_BasePointsCriticalHit = container.Q<Label>(k_BasePointsCriticalHit);

            m_SkillIcons[0] = container.Q(k_Skill1Icon);
            m_SkillIcons[1] = container.Q(k_Skill2Icon);
            m_SkillIcons[2] = container.Q(k_Skill3Icon);

            m_Skill = container.Q<Label>(k_Skill);
            m_Category = container.Q<Label>(k_Category);
            m_Tier = container.Q<Label>(k_Tier);
            m_Damage = container.Q<Label>(k_Damage);
            m_NextTier = container.Q<Label>(k_NextTier);

            m_NextSkillButton = container.Q<Button>(k_NextSkillButton);
            m_LastSkillButton = container.Q<Button>(k_LastSkillButton);

            m_BioTitle = container.Q<Label>(k_BioTitle);
            m_BioText = container.Q<Label>(k_BioText);

            m_ActiveFrame = container.Q(k_ActiveFrame);

        }

        // assign data to each UI Element
        public void SetGameData()
        {
            m_BaseSkills[0] = m_BaseStats.skill1;
            m_BaseSkills[1] = m_BaseStats.skill2;
            m_BaseSkills[2] = m_BaseStats.skill3;

            // set level from CharacterData...
            uint levelNumber = (uint) m_CharacterData.CurrentLevel;
            m_LevelLabel.text = "Level " + levelNumber;

            // class/rarity/attackType
            m_CharacterClass.text = m_BaseStats.characterClass.ToString();
            m_Rarity.text = m_BaseStats.rarity.ToString();
            m_AttackType.text = m_BaseStats.attackType.ToString();

            // TO-DO: missing data validation
            Sprite charClassSprite = m_GameIconsData.GetCharacterClassIcon(m_BaseStats.characterClass);
            m_CharacterClassIcon.style.backgroundImage = new StyleBackground(charClassSprite);

            Sprite raritySprite = m_GameIconsData.GetRarityIcon(m_BaseStats.rarity);
            m_RarityIcon.style.backgroundImage = new StyleBackground(raritySprite);

            Sprite attackTypeSprite = m_GameIconsData.GetAttackTypeIcon(m_BaseStats.attackType);
            m_AttackTypeIcon.style.backgroundImage = new StyleBackground(attackTypeSprite);

            // base points
            m_BasePointsLife.text = m_BaseStats.basePointsLife.ToString();
            m_BasePointsDefense.text = m_BaseStats.basePointsDefense.ToString();
            m_BasePointsAttack.text = m_BaseStats.basePointsAttack.ToString();
            m_BasePointsAttackSpeed.text = m_BaseStats.basePointsAttackSpeed.ToString() + "/s";
            m_BasePointsSpecialAttack.text = m_BaseStats.basePointsSpecialAttack.ToString() + "/s";
            m_BasePointsCriticalHit.text = m_BaseStats.basePointsCriticalHit.ToString();

            // bio
            m_BioTitle.text = m_BaseStats.bioTitle;
            m_BioText.text = m_BaseStats.bio;


            if (m_BaseStats.skill1 != null && m_BaseStats.skill2 != null && m_BaseStats.skill3 != null)
            {
                m_SkillIcons[0].style.backgroundImage = new StyleBackground(m_BaseStats.skill1.sprite);
                m_SkillIcons[1].style.backgroundImage = new StyleBackground(m_BaseStats.skill2.sprite);
                m_SkillIcons[2].style.backgroundImage = new StyleBackground(m_BaseStats.skill3.sprite);
            }
            else
            {
                Debug.LogWarning("CharStatsWindow.SetGameData: " + m_CharacterData.CharacterBaseData.characterName +
                    " missing Skill ScriptableObject(s)");
                return;
            }
        }

        // set up interactive UI elements
        public void RegisterCallbacks()
        {
            m_NextSkillButton?.RegisterCallback<ClickEvent>(SelectNextSkill);
            m_LastSkillButton?.RegisterCallback<ClickEvent>(SelectLastSkill);
            m_SkillIcons[0]?.RegisterCallback<ClickEvent>(SelectSkill);
            m_SkillIcons[1]?.RegisterCallback<ClickEvent>(SelectSkill);
            m_SkillIcons[2]?.RegisterCallback<ClickEvent>(SelectSkill);

            // initialize the active frame position after the layout builds
            m_SkillIcons[0]?.RegisterCallback<GeometryChangedEvent>(InitializeSkillMarker);

            // update the text
            ShowSkill(0);
        }

        void SelectLastSkill(ClickEvent evt)
        {
            if (Time.time < timeToNextClick)
                return;

            timeToNextClick = Time.time + clickCooldown;

            // only select when clicking directly on the visual element
            m_ActiveIndex--;
            if (m_ActiveIndex < 0)
            {
                m_ActiveIndex = 2;
            }
            ShowSkill(m_ActiveIndex);
            AudioManager.PlayDefaultButtonSound();
        }

        void SelectNextSkill(ClickEvent evt)
        {
            if (Time.time < timeToNextClick)
                return;

            timeToNextClick = Time.time + clickCooldown;
            m_ActiveIndex++;
            if (m_ActiveIndex > 2)
            {
                m_ActiveIndex = 0;
            }
            ShowSkill(m_ActiveIndex);
            AudioManager.PlayDefaultButtonSound();
        }

        void SelectSkill(ClickEvent evt)
        {
            VisualElement clickedElement = evt.target as VisualElement;

            int index = (int) Char.GetNumericValue(clickedElement.name[clickedElement.name.Length - 1]) - 1;
            index = Mathf.Clamp(index, 0, m_BaseSkills.Length - 1);
            
            ShowSkill(index);

            AudioManager.PlayAltButtonSound();
        }

        void ShowSkill(int index)
        {
            SkillSO skill = m_BaseSkills[index];
            if (skill != null)
            {
                SetSkillData(skill);
                MarkTargetElement(m_SkillIcons[index], 300);
                m_ActiveIndex = index;
            }
        }

        // show the description for a given skill
        void SetSkillData(SkillSO skill)
        {
            m_Skill.text = skill.skillName;
            m_Category.text = skill.GetCategoryText();
            m_Tier.text = skill.GetTierText((int)m_CharacterData.CurrentLevel);
            m_Damage.text = skill.GetDamageText();
            m_NextTier.text = skill.GetNextTierLevelText((int)m_CharacterData.CurrentLevel);
        }

        // set up the active frame after the layout builds
        void InitializeSkillMarker(GeometryChangedEvent evt)
        {
            // set its position over the first icon
            MarkTargetElement(m_SkillIcons[0], 0);
        }

        void MarkTargetElement(VisualElement targetElement, int duration = 200)
        {
            // target element, converted into the root space of the Active Frame
            Vector3 targetInRootSpace = ElementInRootSpace(targetElement, m_ActiveFrame);

            // padding offset
            Vector3 offset = new Vector3(10, 10, 0f);

            m_ActiveFrame?.experimental.animation.Position(targetInRootSpace - offset, duration);
        }

        // convert target VisualElement into another element's parent space 
        Vector3 ElementInRootSpace(VisualElement targetElement, VisualElement newElement)
        {
            Vector2 targetInWorldSpace = targetElement.parent.LocalToWorld(targetElement.layout.position);
            VisualElement newRoot = newElement.parent;
            return newRoot.WorldToLocal(targetInWorldSpace);
        }
    }
}