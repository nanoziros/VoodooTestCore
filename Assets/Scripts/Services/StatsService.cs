using System.Collections.Generic;
using Configs;
using Gameplay;
using Interfaces.Services;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Services
{
	public class StatsService : IStatsService
	{
		public int FavoriteSkin
		{
			get
			{
				return PlayerPrefs.GetInt("FavoriteSkin", 0);
			}
			set
			{
				PlayerPrefs.SetInt("FavoriteSkin", value);
			}
		}

		private StatsConfig m_StatsConfig;
		private Dictionary<GameMode, int> m_LastGain = new Dictionary<GameMode, int>();

		[Inject]
		public void Construct(StatsConfig statsConfig)
		{
			m_StatsConfig = statsConfig;
		}

		public int GetLastGain(GameMode gameMode)
		{
			return m_LastGain.GetValueOrDefault(gameMode, 0);
		}

		private int GetGameResult(GameMode gameMode, int _Index)
		{
			string key = Constants.GetGameResultSaveId(gameMode) + "_" + _Index;

			if (PlayerPrefs.HasKey(key))
				return PlayerPrefs.GetInt(key);
			return 0;
		}

		public void AddGameResult(GameMode gameMode, int _WinScore)
		{
			// Move results
			string gameResultGameId = Constants.GetGameResultSaveId(gameMode);
			for (int i = Constants.c_SavedGameCount - 1; i >= 0; --i)
			{
				string key = gameResultGameId + "_" + i.ToString ();
				PlayerPrefs.SetInt (key, GetGameResult (gameMode,i - 1));
			}

			// Set new result
			PlayerPrefs.SetInt (Constants.GetGameResultSaveId(gameMode) + "_0", _WinScore);
		}

		public float GetLevel(GameMode gameMode)
		{
			int result = 0;

			for (int i = 0; i < Constants.c_SavedGameCount; ++i)
				result += GetGameResult (gameMode, i);
            
			float percent = ((float)result) / Constants.c_SavedGameCount;
			return Mathf.Clamp01(percent);
		}

		public void TryToSetBestScore(GameMode gameMode, int _Score)
		{
			int score = GetBestScore (gameMode);
			if (score < _Score)
			{
				PlayerPrefs.SetInt(Constants.GetBestScoreId(gameMode), _Score);
			}
		}

		private int GetBestScore(GameMode gameMode)
		{
			string bestScoreId = Constants.GetBestScoreId(gameMode);
			if (PlayerPrefs.HasKey(bestScoreId))
				return PlayerPrefs.GetInt(bestScoreId);
			else
				return 0;
		}

		public void SetNickname(string _Name)
		{
			PlayerPrefs.SetString(Constants.c_PlayerNameSave, _Name);
		}

		public string GetNickname()
		{
			return (PlayerPrefs.GetString(Constants.c_PlayerNameSave, null));
		}

		public void SetLastXP(GameMode gameMode, int _XP)
		{
			m_LastGain[gameMode] = _XP;
		}

		public void GainXP(GameMode gameMode)
		{
			int _XP  = 0;
			if (m_LastGain.TryGetValue(gameMode, out var value))
			{
				_XP = value;
			}

			int xp = _XP + GetXP(gameMode);

			while (xp >= XPToNextLevel(gameMode))
			{
				xp -= XPToNextLevel(gameMode);
				LevelUp(gameMode);
			}
			
			PlayerPrefs.SetInt(Constants.GetPlayerXpId(gameMode), xp);
		}
    
		public int GetXP(GameMode gameMode)
		{
			return PlayerPrefs.GetInt(Constants.GetPlayerXpId(gameMode), 0);
		}

		public int GetPlayerLevel(GameMode gameMode)
		{
			return PlayerPrefs.GetInt(Constants.GetPlayerLevelSaveId(gameMode), 1);
		}

		void LevelUp(GameMode gameMode)
		{
			PlayerPrefs.SetInt(Constants.GetPlayerLevelSaveId(gameMode), GetPlayerLevel(gameMode) + 1);
		}
		
		void LevelDown(GameMode gameMode)
		{
			PlayerPrefs.SetInt(Constants.GetPlayerLevelSaveId(gameMode), GetPlayerLevel(gameMode) - 1);
		}

		public int XPToNextLevel(GameMode gameMode, int _LevelStart = -1)
		{
			int currentLevel = _LevelStart == -1 ? GetPlayerLevel(gameMode) - 1 : _LevelStart;
			var xpProgression = GetXPForLevelProgression(gameMode);
			int index = Mathf.Min(currentLevel, xpProgression.Count - 1);
			return xpProgression[index];
		}

		private List<int> GetXPForLevelProgression(GameMode gameMode)
		{
			switch (gameMode)
			{
				case GameMode.BOOSTER:
					return m_StatsConfig.m_XPForBoosterLevel;
				default:
					return m_StatsConfig.m_XPForLevel;
			}
		}
		
		#region IAs

		// Behaviour probas
		private const int 			c_MaxRandomProbaLevel = 50;
		private const float 		c_FirstMinRandomProba = 0.1f;
		private const float 		c_FirstMaxRandomProba = 0.2f;
		private const float 		c_SecondMinRandomProba = 0.15f;
		private const float 		c_SecondMaxRandomProba = 0.3f;

		// Random duration
		private const int 			c_MaxRandomDurationLevel = 50;
		private const float 		c_FirstMinRandomDuration = 10.0f;
		private const float 		c_FirstMaxRandomDuration = 20.0f;
		private const float 		c_SecondMinRandomDuration = 5.0f;
		private const float 		c_SecondMaxRandomDuration = 10.0f;

		public AnimationCurve		m_RandomProbaCurve;
		public AnimationCurve		m_RandomDurationCurve;

		public float GetRandomProba(GameMode gameMode)
		{
			return GetRandomValue (gameMode, c_MaxRandomProbaLevel, c_FirstMinRandomProba, c_FirstMaxRandomProba, c_SecondMinRandomProba, c_SecondMaxRandomProba, m_RandomProbaCurve);
		}

		public float GetRandomDuration(GameMode gameMode)
		{
			return GetRandomValue (gameMode, c_MaxRandomDurationLevel, c_FirstMinRandomDuration, c_FirstMaxRandomDuration, c_SecondMinRandomDuration, c_SecondMaxRandomDuration, m_RandomDurationCurve);
		}

		private float GetRandomValue(GameMode gameMode, int _MaxLevel, float _FirstMin, float _FirstMax, float _SecondMin, float _SecondMax, AnimationCurve _Curve)
		{
			float level = GetLevel(gameMode);
			float percent = _Curve.Evaluate(level / ((float)_MaxLevel));
			float minValue = Mathf.Lerp(_FirstMin, _SecondMin, _Curve.Evaluate(percent));
			float maxValue = Mathf.Lerp(_FirstMax, _SecondMax, _Curve.Evaluate(percent));
			return Random.Range(minValue, maxValue);
		}

		#endregion
	}
}
