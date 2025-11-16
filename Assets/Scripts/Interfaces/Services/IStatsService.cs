using System.Collections.Generic;
using Services;
namespace Interfaces.Services
{
    public interface IStatsService
    {
        public float GetLevel(GameMode gameMode);
        void TryToSetBestScore(GameMode gameMode, int playerScore);
        void AddGameResult(GameMode gameMode, int rankingScore);
        void SetLastXP(GameMode _GameMode, int xp);
        void GainXP(GameMode gameMode);
        int FavoriteSkin { get; set; }
        int GetLastGain (GameMode gameMode);
        int GetXP(GameMode gameMode);
        int GetPlayerLevel(GameMode gameMode);
        int XPToNextLevel(GameMode gameMode, int currentLevel);
        string GetNickname();
        void SetNickname(string name);
    }
}
