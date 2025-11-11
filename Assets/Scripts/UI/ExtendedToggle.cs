using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ExtendedToggle : MonoBehaviour
    {
        public Toggle m_Toggle;
        public Text m_ToggleLabel;
        public Image m_ToggleImage;

        public Color m_OnColor = Color.green;
        public Color m_OffColor = Color.red;
        
        public string m_OnText = "ON";
        public string m_OffText = "OFF";
        
        private string m_Title;
        
        private void Start()
        {
            m_Title = m_ToggleLabel.text;

            UpdateToggleUI(m_Toggle.isOn);

            m_Toggle.onValueChanged.AddListener(UpdateToggleUI);
        }

        void UpdateToggleUI(bool isOn)
        {
            m_ToggleImage.color = isOn ? m_OnColor : m_OffColor;
            
            // todo: localization can be integrated here
            m_ToggleLabel.text = $"{m_Title} : {(isOn ? m_OnText : m_OffText)}";
        }
    }
}
