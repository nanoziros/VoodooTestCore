using Interfaces.Services;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class RankingView : MonoBehaviour
    {
        public Text m_PreviousRank;
        public Text m_ActualRank;
        public Text m_NextRank;

        public Text m_CurrentXPText;
        public Text m_CurrentLevelText;
        public Text m_NextLevelText;
        public Image m_XPBar;
        public Image m_RankIcon;

        public Text m_LevelCrownText;

        private IStatsService m_StatsService;

        [Inject]
        public void Construct(IStatsService statsService)
        {
            m_StatsService = statsService;
        }

        public void RefreshNormal()
        {
            int playerLevel = m_StatsService.GetPlayerLevel(GameMode.NORMAL);
            int currentXp = m_StatsService.GetXP(GameMode.NORMAL);
            int nextLevelXp = m_StatsService.XPToNextLevel(GameMode.NORMAL, playerLevel - 1);
            
            int tmpLvl = playerLevel- 1;
            if (tmpLvl >= MainMenuView.Instance.m_Ratings.Length)
            {
                tmpLvl = MainMenuView.Instance.m_Ratings.Length - 1;
            }

            m_CurrentXPText.text = currentXp + "/" + nextLevelXp;
            m_CurrentLevelText.text = playerLevel.ToString();
            m_LevelCrownText.text = "LVL" + playerLevel;
            m_NextLevelText.text = (playerLevel + 1).ToString();

            float levelPercent = currentXp / (float)nextLevelXp;
            m_XPBar.rectTransform.anchorMax = new Vector2(levelPercent, 1f);
            m_XPBar.rectTransform.anchoredPosition = Vector2.zero;
            m_XPBar.gameObject.SetActive(levelPercent > 0.02f);

            m_PreviousRank.text = GetNameByRank(tmpLvl - 1);
            m_ActualRank.text = GetNameByRank(tmpLvl);
            m_NextRank.text = GetNameByRank(tmpLvl + 1);
        }

        private string GetNameByRank(int _Rank)
        {
            if (_Rank < 0)
                return "";
            if (_Rank > MainMenuView.Instance.GetRankingCount() - 1)
                return "";
            return MainMenuView.Instance.GetRanking(_Rank);
        }

    }
}
