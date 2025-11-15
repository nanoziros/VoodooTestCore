using System.Collections.Generic;
using Gameplay.Data;
namespace Configs
{
    [System.Serializable]
    public class LevelPowerUpConfig
    {
        public List<PowerUpData> m_EnabledPowerUps = new List<PowerUpData>();
    }
}
