using Interfaces.Services;
using Services;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using Zenject;
namespace UI
{
    public class DebugMenuView: View<DebugMenuView>
    {
        public Toggle m_BoosterModeToggle;
        public Toggle m_SkinChangeToggle;
        public Button m_resetPlayerButton;
   
        private IFeatureFlagService  m_FeatureFlagService;
        
        [Inject]
        public void Construct(IFeatureFlagService featureFlagService)
        {
            m_FeatureFlagService = featureFlagService;
        }
        
        protected override void Awake()
        {
            base.Awake();

            // Bind booster mode feature toggle
            m_BoosterModeToggle.onValueChanged.AddListener(newState =>
            {
                m_FeatureFlagService.BoosterGameModeEnabled = newState;
            });
            m_BoosterModeToggle.isOn = m_FeatureFlagService.BoosterGameModeEnabled;
            
            // Bind skin change feature toggle
            m_SkinChangeToggle.onValueChanged.AddListener(newState =>
            {
                m_FeatureFlagService.SkinSelectionScreenEnabled = newState;
            });
            m_SkinChangeToggle.isOn = m_FeatureFlagService.SkinSelectionScreenEnabled;
            
            // Bind reset player button
            m_resetPlayerButton.onClick.AddListener(() =>
            {
                PlayerPrefsUtility.DeleteAll();
                OnClickContinueButton();
            });
        }
        
        public void OnClickContinueButton()
        {
            SceneLoaderUtility.LoadMainMenu();
        }

        protected override void OnGamePhaseChanged(GamePhase _GamePhase)
        {
            base.OnGamePhaseChanged(_GamePhase);
            switch (_GamePhase)
            {
                case GamePhase.DEBUG:
                    Transition(true);
                    break;
            }
        }
    }
}