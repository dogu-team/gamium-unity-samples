using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    public class HealthBarComponent : VisualElement
    {
       private static class ClassNames
       {
           
           public static string HealthBarBackground = "health-bar__background";
           public static string HealthBarProgress = "health-bar__progress";
           public static string HealthBarTitle = "health-bar__title";
           public static string HealthBarLabel = "health-bar__label";
           public static string HealthBarContainer = "health-bar__container";
           public static string HealthBarTitleBackground = "health-bar__title_background";
       }

       public new class UxmlFactory : UxmlFactory<HealthBarComponent, UxmlTraits>
       {
       }

       public new class UxmlTraits : VisualElement.UxmlTraits
       {
           readonly UxmlIntAttributeDescription _currentHealth = new UxmlIntAttributeDescription
               {name = "currentHealth", defaultValue = 0};

           readonly UxmlIntAttributeDescription _mininumHealth = new UxmlIntAttributeDescription
               {name = "MinimumHealth", defaultValue = 0};

           readonly UxmlIntAttributeDescription _maximumHealth = new UxmlIntAttributeDescription
               {name = "MaximumHealth", defaultValue = 100};

           readonly UxmlStringAttributeDescription _healthBarTitle = new UxmlStringAttributeDescription
               {name = "HealthBarTitle", defaultValue = string.Empty};

           public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
           {
               base.Init(ve, bag, cc);
               var healthBar = (HealthBarComponent) ve;
               healthBar.CurrentHealth = _currentHealth.GetValueFromBag(bag, cc);
               healthBar.MinimumHealth = _mininumHealth.GetValueFromBag(bag, cc);
               healthBar.MaximumHealth = _maximumHealth.GetValueFromBag(bag, cc);
               healthBar.HealthBarTitle = _healthBarTitle.GetValueFromBag(bag, cc);
           }
       }
       //Must have properties that are named the same as the defined Traits in order for data to be properly
       //linked and displayed in the Builder 
       private int _currentHealth;
       private int _minimumHealth;
       private int _maximumHealth;
       private string _healthBarTitle;
       private readonly Label _titleLabel;
       private readonly Label _healthStat;
       private VisualElement _progress;
       private VisualElement _background;
       private VisualElement _titleBackground;
       
       public int CurrentHealth
       {
           get => _currentHealth;
           set
           {
               if (value == _currentHealth)
                   return;
               _currentHealth = value;
               SetHealth(_currentHealth, _maximumHealth);
           }
       }
       public int MinimumHealth
       {
           get => _minimumHealth;
           set => _minimumHealth = value;
       }
       public int MaximumHealth
       {
           get => _maximumHealth;
           set
           {
               if (value == _maximumHealth)
                   return;
               _maximumHealth = value;
               SetHealth(_currentHealth, _maximumHealth);
           }
       }

       public string HealthBarTitle
       {
           get=> _healthBarTitle;
           set => _titleLabel.text = value;
       }

       public HealthBarComponent()
       {
           _titleBackground = new VisualElement {name = "HealthBarTitleBackground"};
           _titleBackground.AddToClassList(ClassNames.HealthBarTitleBackground);
           Add(_titleBackground);
           
           _titleLabel = new Label() {name = "HealthBarTitle"};
           _titleLabel.AddToClassList(ClassNames.HealthBarTitle);
           _titleBackground.Add(_titleLabel);
           
           AddToClassList(ClassNames.HealthBarContainer);
           //Add Elements and their class selectors to the Component.
           _background = new VisualElement {name = "HealthBarBackground"};
           _background.AddToClassList(ClassNames.HealthBarBackground);
           Add(_background);

           _progress = new VisualElement {name = "HealthBarProgress"};
           _progress.AddToClassList(ClassNames.HealthBarProgress);
           _background.Add(_progress);

           _healthStat = new Label() {name = "HealthBarStat"};
           _healthStat.AddToClassList(ClassNames.HealthBarLabel);
           _progress.Add(_healthStat);
       }

       private void SetHealth(int currentHealth, int maxHealth)
       {
           _healthStat.text = $"{currentHealth}/{maxHealth}";
           if (maxHealth > 0)
           {
               float w = Mathf.Clamp((float) currentHealth / maxHealth * 100, 0f, 100f);
               _progress.style.width = new StyleLength(Length.Percent(w));
           }
       }

      
    }
}
