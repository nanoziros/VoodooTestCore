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

		public int GetLastGain(GameMode _GameMode)
		{
			return m_LastGain.GetValueOrDefault(_GameMode, 0);
		}

		private int GetGameResult(GameMode _GameMode, int _Index)
		{
			string key = Constants.GetGameResultSaveId(_GameMode) + "_" + _Index;

			return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : 0;
		}

		public void AddGameResult(GameMode _GameMode, int _WinScore)
		{
			// Move results
			string gameResultGameId = Constants.GetGameResultSaveId(_GameMode);
			for (int i = Constants.c_SavedGameCount - 1; i >= 0; --i)
			{
				string key = gameResultGameId + "_" + i.ToString ();
				PlayerPrefs.SetInt (key, GetGameResult (_GameMode,i - 1));
			}

			// Set new result
			PlayerPrefs.SetInt (Constants.GetGameResultSaveId(_GameMode) + "_0", _WinScore);
		}

		public float GetLevel(GameMode _GameMode)
		{
			int result = 0;

			for (int i = 0; i < Constants.c_SavedGameCount; ++i)
				result += GetGameResult (_GameMode, i);
            
			float percent = ((float)result) / Constants.c_SavedGameCount;
			return Mathf.Clamp01(percent);
		}

		public void TryToSetBestScore(GameMode _GameMode, int _Score)
		{
			int score = GetBestScore (_GameMode);
			if (score < _Score)
			{
				PlayerPrefs.SetInt(Constants.GetBestScoreId(_GameMode), _Score);
			}
		}

		private int GetBestScore(GameMode _GameMode)
		{
			string bestScoreId = Constants.GetBestScoreId(_GameMode);
			return PlayerPrefs.HasKey(bestScoreId) ? PlayerPrefs.GetInt(bestScoreId) : 0;
		}

		public void SetNickname(string _Name)
		{
			PlayerPrefs.SetString(Constants.c_PlayerNameSave, _Name);
		}

		public string GetNickname()
		{
			return (PlayerPrefs.GetString(Constants.c_PlayerNameSave, null));
		}

		public void SetLastXP(GameMode _GameMode, int _XP)
		{
			m_LastGain[_GameMode] = _XP;
		}

		public void GainXP(GameMode _GameMode)
		{
			int _XP  = 0;
			if (m_LastGain.TryGetValue(_GameMode, out var value))
			{
				_XP = value;
			}

			int xp = _XP + GetXP(_GameMode);

			while (xp >= XPToNextLevel(_GameMode))
			{
				xp -= XPToNextLevel(_GameMode);
				LevelUp(_GameMode);
			}
			
			PlayerPrefs.SetInt(Constants.GetPlayerXpId(_GameMode), xp);
		}
    
		public int GetXP(GameMode _GameMode)
		{
			return PlayerPrefs.GetInt(Constants.GetPlayerXpId(_GameMode), 0);
		}

		public int GetPlayerLevel(GameMode _GameMode)
		{
			return PlayerPrefs.GetInt(Constants.GetPlayerLevelSaveId(_GameMode), 1);
		}

		void LevelUp(GameMode _GameMode)
		{
			PlayerPrefs.SetInt(Constants.GetPlayerLevelSaveId(_GameMode), GetPlayerLevel(_GameMode) + 1);
		}
		
		void LevelDown(GameMode _GameMode)
		{
			PlayerPrefs.SetInt(Constants.GetPlayerLevelSaveId(_GameMode), GetPlayerLevel(_GameMode) - 1);
		}

		public int XPToNextLevel(GameMode _GameMode, int _LevelStart = -1)
		{
			int currentLevel = _LevelStart == -1 ? GetPlayerLevel(_GameMode) - 1 : _LevelStart;
			var xpProgression = GetXPForLevelProgression(_GameMode);
			int index = Mathf.Min(currentLevel, xpProgression.Count - 1);
			return xpProgression[index];
		}

		private List<int> GetXPForLevelProgression(GameMode _GameMode)
		{
			switch (_GameMode)
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

		public float GetRandomProba(GameMode _GameMode)
		{
			return GetRandomValue (_GameMode, c_MaxRandomProbaLevel, c_FirstMinRandomProba, c_FirstMaxRandomProba, c_SecondMinRandomProba, c_SecondMaxRandomProba, m_RandomProbaCurve);
		}

		public float GetRandomDuration(GameMode _GameMode)
		{
			return GetRandomValue (_GameMode, c_MaxRandomDurationLevel, c_FirstMinRandomDuration, c_FirstMaxRandomDuration, c_SecondMinRandomDuration, c_SecondMaxRandomDuration, m_RandomDurationCurve);
		}

		private float GetRandomValue(GameMode _GameMode, int _MaxLevel, float _FirstMin, float _FirstMax, float _SecondMin, float _SecondMax, AnimationCurve _Curve)
		{
			float level = GetLevel(_GameMode);
			float percent = _Curve.Evaluate(level / ((float)_MaxLevel));
			float minValue = Mathf.Lerp(_FirstMin, _SecondMin, _Curve.Evaluate(percent));
			float maxValue = Mathf.Lerp(_FirstMax, _SecondMax, _Curve.Evaluate(percent));
			return Random.Range(minValue, maxValue);
		}

		#endregion
	}
}
