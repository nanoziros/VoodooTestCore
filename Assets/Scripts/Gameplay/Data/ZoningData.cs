using UnityEngine;

namespace Gameplay.Data
{
	[CreateAssetMenu(fileName = "Zoning", menuName = "Data/Zoning", order = 1)]
	public class ZoningData : ScriptableObject 
	{
		public int		m_Order = 0;
		public Color 	m_TopBgColor;
		public Color 	m_BottomBgColor;
	}
}
