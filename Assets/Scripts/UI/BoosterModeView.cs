
using Interfaces.Services;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace UI
{
    public class BoosterModeView: MonoBehaviour
    {
        public Button m_BoosterButton;
        public TextMeshProUGUI m_LevelText;
        
        private IStatsService m_StatsService;
        
        [Inject]
        public void Construct(IStatsService statsService)
        {
            m_StatsService = statsService;
        }

        void Awake()
        {
            m_BoosterButton.onClick.AddListener(MainMenuView.Instance.OnBoosterModeButton);
            
            m_LevelText.text = $"Lvl {m_StatsService.GetPlayerLevel(GameMode.BOOSTER)}";
        }
    }
}
