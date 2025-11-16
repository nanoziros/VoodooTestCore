using System.Collections.Generic;
using Services;
using UnityEngine;

namespace Gameplay.Data
{
	[CreateAssetMenu(fileName = "PowerUp", menuName = "Data/PowerUp", order = 1)]
	public class PowerUpData : ScriptableObject
	{
		public GameObject 	m_Prefab;
		public List<GameMode> m_ExclusiveToGameMode = new List<GameMode>();
	}
}
