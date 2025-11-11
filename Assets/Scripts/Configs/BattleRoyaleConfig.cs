using Gameplay.PowerUps;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "BattleRoyaleConfig", menuName = "Config/BattleRoyaleConfig")]
    public class BattleRoyaleConfig : ScriptableObject
    {
        public GameObject 							m_Crown;
        public GameObject							m_Skull;
        public PowerUp_DeadMan                      m_PowerUpOnDeathPrefab;
    }
}
