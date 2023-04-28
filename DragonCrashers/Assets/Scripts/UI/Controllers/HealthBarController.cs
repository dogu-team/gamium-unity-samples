using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    [RequireComponent(typeof(UIDocument))]
    public class HealthBarController : MonoBehaviour
    {

        [Header("HealthBar Elements")]
        [SerializeField] string m_HealthBarName = "HealthBarBase";
        [SerializeField] string m_CharacterName = "Character Name";

        [SerializeField] Vector2 m_WorldSize = new Vector2(1.2f, 0.6f);
        [SerializeField] bool m_ShowStat = true;
        [SerializeField] bool m_ShowNameplate = true;
        [SerializeField] StyleSheet m_StyleSheetOverride;

        [SerializeField] float m_LowHPPercent = 25;
        [SerializeField] Sprite m_LowHPImage;
        [SerializeField] Transform transformToFollow;

        // string IDs
        const string k_NamePlate = "HealthBarTitleBackground";
        const string k_Stat = "HealthBarStat";
        const string k_HPFillImage = "HealthBarProgress";

        HealthBarComponent m_HealthBar;
        StyleBackground m_FillStyleBackground;
        UIDocument m_HealthBarDoc;

        void HealthBarSetup()
        {
            m_HealthBarDoc = GetComponent<UIDocument>();
            VisualElement rootElement = m_HealthBarDoc.rootVisualElement;
            if (m_StyleSheetOverride != null)
            {
                rootElement.styleSheets.Clear();
                rootElement.styleSheets.Add(m_StyleSheetOverride);
            }
            m_HealthBar = rootElement.Q<HealthBarComponent>(m_HealthBarName);
            m_HealthBar.HealthBarTitle = m_CharacterName;

            ShowNameAndStats(m_ShowNameplate, m_ShowStat);
            MoveToWorldPosition(m_HealthBar, transformToFollow.position, m_WorldSize);

        }

        public void DisplayHealthBar(bool state)
        {
            if (m_HealthBarDoc == null)
                return;

            VisualElement rootElement = m_HealthBarDoc.rootVisualElement;
            rootElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
        }


        public void SetHealth(int health, int maxHealth)
        {
            if (m_HealthBar == null)
            {
                HealthBarSetup();
            }
            m_HealthBar.CurrentHealth = health;
            m_HealthBar.MaximumHealth = maxHealth;
        }

        public void UpdateHealth(int health)
        {
            float lowHealth = m_HealthBar.MaximumHealth * m_LowHPPercent / 100;
            if (health < lowHealth && m_LowHPImage != null)
            {
                VisualElement fill = m_HealthBar.Q<VisualElement>(k_HPFillImage);
                fill.style.backgroundImage = new StyleBackground(m_LowHPImage);
            }
            else
            {
                VisualElement fill = m_HealthBar.Q<VisualElement>(k_HPFillImage);
                fill.style.backgroundImage = m_FillStyleBackground;
            }

            m_HealthBar.CurrentHealth = health;
        }

        void ShowNameAndStats(bool nameVisible, bool statVisible)
        {
            VisualElement nameplate = m_HealthBar.Q<VisualElement>(k_NamePlate);
            VisualElement stat = m_HealthBar.Q<Label>(k_Stat);

            if (nameplate != null)
            {
                nameplate.visible = nameVisible;
            }

            if (stat != null)
            {
                stat.visible = statVisible;
            }

            // store the background style to reset the health bar sprite
            m_FillStyleBackground = m_HealthBar.Q<VisualElement>(k_HPFillImage).style.backgroundImage;
 
        }

        // moves health bar to match world position
        static void MoveToWorldPosition(VisualElement element, Vector3 worldPosition, Vector2 worldSize)
        {
            Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, worldSize, Camera.main);
            Vector2 layoutSize = element.layout.size;

            // Don't set scale to 0 or a negative number.
            // Vector2 scale = layoutSize.x > 0 && layoutSize.y > 0 ? rect.size / layoutSize : Vector2.one * 1e-5f;

            element.transform.position = rect.position;
            //element.transform.scale = new Vector3(scale.x, scale.y, 1);
        }
    }
}
