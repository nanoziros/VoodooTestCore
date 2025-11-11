using Gameplay.Terrain;
using UnityEngine;

namespace Gameplay.Data
{
	[CreateAssetMenu(fileName = "Terrain", menuName = "Data/Terrain", order = 1)]
	public class TerrainData : ScriptableObject
	{
		public TerrainController m_Prefab;
	}
}
