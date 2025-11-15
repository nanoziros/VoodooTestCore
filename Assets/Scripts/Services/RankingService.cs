using System.Collections.Generic;
using Configs;
using Gameplay;
using Gameplay.Data;
using Interfaces.Services;
using UnityEngine;
using Zenject;

namespace Services
{
    public class RankingService : IRankingService 
    {
        public int m_PointsPerRank => m_RankingConfig.m_PointsPerRank;
        public List<int> m_XPByRank => m_RankingConfig.m_XPByRank;

        public int m_MaxRanks => m_RankingConfig.m_MaxRanks;

        public int m_LastGain;

        public List<RankData> m_RankData;

        private RankingConfig m_RankingConfig;
    
        [Inject]
        public void Construct(RankingConfig rankingConfig)
        {
            m_RankingConfig = rankingConfig;
            Init();
        }
    
        private void Init()
        {
            m_RankData = new List<RankData>(Resources.LoadAll<RankData>("Ranks"));
        }

        public int GetPoints(GameMode gameMode)
        {
            return (PlayerPrefs.GetInt(Constants.GetPlayerXpId(gameMode), 0));
        }

        private void LevelUp(GameMode gameMode)
        {
            PlayerPrefs.SetInt(Constants.GetPlayerLevelSaveId(gameMode), GetPlayerLevel(gameMode) + 1);
        }

        private void LevelDown(GameMode gameMode)
        {
            PlayerPrefs.SetInt(Constants.GetPlayerLevelSaveId(gameMode), GetPlayerLevel(gameMode) - 1);
        }

        public RankData GetRank(int _Level)
        {
            return (m_RankData[Mathf.Clamp(_Level, 0, m_RankData.Count - 1)]);
        }

        public int GetPlayerLevel(GameMode gameMode)
        {
            return (PlayerPrefs.GetInt(Constants.GetPlayerLevelSaveId(gameMode), 1));
        }
    }
}
