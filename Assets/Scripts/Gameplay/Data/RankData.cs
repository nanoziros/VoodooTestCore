using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(fileName = "Rank", menuName = "Data/Rank", order = 1)]
    public class RankData : ScriptableObject
    {
        public Sprite m_Icon;
        public string m_RankName;
    }
}
