using System;
using System.Collections.Generic;
using Gameplay.Players;

namespace Interfaces.Services
{
    public interface IBattleRoyaleService
    {
        event Action<Player> onElimination;
        public Player GetHumanPlayer();
        void SetPlayers(List<Player> players);
        void SetHumanPlayer(Player player);
        int GetAlivePlayersCount();
        void KillHumanPlayer();
        bool m_IsPlaying { get; set; }
        List<Player> m_Players { get; set; }
        void Order();
        Player GetBestPlayer();
        Player GetPlayer(int ranking);
        float GetTimeBeforeNextElimination();
        void ApplySaveMechanic(Player humanPlayer);
        Player GetWorstPlayer();
    }
}
