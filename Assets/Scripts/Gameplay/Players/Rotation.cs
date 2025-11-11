using UnityEngine;

namespace Gameplay.Players
{
	public class Rotation : MonoBehaviour
	{
		// Cache
		private Transform	m_Transform;

		void Awake ()
		{
			// Cache
			m_Transform = transform;
		}

		void Update ()
		{
			m_Transform.RotateAround (m_Transform.position, Vector3.up, 50.0f * Time.deltaTime);
		}
	}
}
