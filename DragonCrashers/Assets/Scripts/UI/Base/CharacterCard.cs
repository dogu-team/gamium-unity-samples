using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    // used in gameplay, representing one character portrait with bar
    public class CharacterCard: MonoBehaviour
    {
        public TemplateContainer CharacterTemplate;
        public HealthBarComponent CooldownBar;

        const string k_CooldownBar = "game-char__progress-bar";
        const string k_CharPortraitImage = "game-char__image";
        
        private UnitInfoData m_HeroData;
        private VisualTreeAsset m_CharacterVisualTreeAsset;
        
        public UnitInfoData HeroData
        {
            get => m_HeroData;
            set
            {
                m_HeroData = value;   
                VisualElement charPortraitImage = CharacterTemplate.Q<VisualElement>(k_CharPortraitImage);
                charPortraitImage.style.backgroundImage = new StyleBackground(m_HeroData.unitAvatar);
            }
        }
        public VisualTreeAsset CharacterVisualTreeAsset
        {
            get => m_CharacterVisualTreeAsset;
            set => SetupCard(value);
        }
        void SetupCard(VisualTreeAsset visualTree)
        {
            m_CharacterVisualTreeAsset = visualTree;
            CharacterTemplate = m_CharacterVisualTreeAsset.CloneTree();
            CooldownBar = CharacterTemplate.Q<HealthBarComponent>(k_CooldownBar);
        }



    }
}
