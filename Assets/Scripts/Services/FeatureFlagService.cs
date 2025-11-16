using Configs;
using Interfaces.Services;
using Zenject;

namespace Services
{
    public class FeatureFlagService : IFeatureFlagService
    {
        private FeatureFlagsConfig m_FeatureFlagsConfig;
        
        private bool m_SkinSelectionScreenEnabled;
        private bool m_BoosterGameModeEnabled;
        
        public bool BoosterGameModeEnabled
        {
            get
            {
                return m_BoosterGameModeEnabled;
            }
            set
            {
                m_BoosterGameModeEnabled = value;
            }
        }
        
        public bool SkinSelectionScreenEnabled
        {
            get
            {
                return m_SkinSelectionScreenEnabled;
            }
            set
            {
                m_SkinSelectionScreenEnabled = value;
            }
        }

        [Inject]
        public void Construct(FeatureFlagsConfig featureFlagsConfig)
        {
            m_FeatureFlagsConfig = featureFlagsConfig;

            m_BoosterGameModeEnabled = m_FeatureFlagsConfig.m_DefaultBoosterGameModeEnabled;
            m_SkinSelectionScreenEnabled = m_FeatureFlagsConfig.m_DefaultSkinSelectionScreenEnabled;
        }
    }
}