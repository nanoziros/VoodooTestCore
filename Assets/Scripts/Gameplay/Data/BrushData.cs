using UnityEngine;

namespace Gameplay.Data
{
	[CreateAssetMenu(fileName = "Brush", menuName = "Data/Brush", order = 1)]
	public class BrushData : ScriptableObject
	{
		public GameObject 	m_Prefab;
		public GameObject 	m_PreviewPrefab;
	}
}
