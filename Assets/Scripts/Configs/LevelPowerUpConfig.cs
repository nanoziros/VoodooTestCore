using System.Collections.Generic;
using Gameplay.Data;
namespace Configs
{
    [System.Serializable]
    public class LevelPowerUpConfig
    {
        public bool m_BrushPowerUpEnabled = false;
        public List<PowerUpData> m_EnabledPowerUps = new List<PowerUpData>();
    }
}
