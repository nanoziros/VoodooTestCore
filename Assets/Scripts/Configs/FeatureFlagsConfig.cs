using UI;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "FeatureFlagsConfig", menuName = "Config/FeatureFlagsConfig")]
    public class FeatureFlagsConfig : ScriptableObject
    {
        public bool m_DefaultBoosterGameModeEnabled = false;
        public bool m_DefaultSkinSelectionScreenEnabled = false;
    }
}