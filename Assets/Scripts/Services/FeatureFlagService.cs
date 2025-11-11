using Configs;
using Interfaces.Services;
using Utils;
using Zenject;

namespace Services
{
    public class FeatureFlagService : IFeatureFlagService
    {
        private FeatureFlagsConfig m_FeatureFlagsConfig;
        
        [Inject]
        public void Construct(FeatureFlagsConfig featureFlagsConfig)
        {
            m_FeatureFlagsConfig = featureFlagsConfig;
        }
        
        public bool BoosterGameModeEnabled
        {
            get => PlayerPrefsUtility.GetBool("BoosterGameModeEnabled", m_FeatureFlagsConfig.m_DefaultBoosterGameModeEnabled);
            set => PlayerPrefsUtility.SetBool("BoosterGameModeEnabled", value);
        }

        public bool SkinSelectionScreenEnabled 
        {
            get => PlayerPrefsUtility.GetBool("SkinSelectionScreenEnabled", m_FeatureFlagsConfig.m_DefaultSkinSelectionScreenEnabled);
            set => PlayerPrefsUtility.SetBool("SkinSelectionScreenEnabled", value);
        }
    }
}