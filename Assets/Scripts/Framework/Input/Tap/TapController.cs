using UnityEngine;

namespace Framework.Input.Tap
{
	public sealed class TapController : InputController
	{
		// Cache
		private ITapController			m_TapController;

		// Buffers
		private Vector3					m_PosBuffer;

		protected override void Awake ()
		{
			base.Awake ();

			// Cache
			m_TapController = GetComponent<ITapController> ();

			// Buffers
			m_PosBuffer = Vector3.zero;
		}

		void Update ()
		{
			if (UnityEngine.Input.GetMouseButtonDown(0))
			{
				switch (m_PosType)
				{
					case PositionType.SCREEN:
						m_PosBuffer.Set (UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, 0.0f);
						m_TapController.OnTap (m_PosBuffer);
						break;

					case PositionType.SCREEN_PERCENT:
						m_PosBuffer.Set (UnityEngine.Input.mousePosition.x / Screen.width, UnityEngine.Input.mousePosition.y / Screen.height, 0.0f);
						m_TapController.OnTap (m_PosBuffer);
						break;

					case PositionType.WORLD:
						m_PosBuffer.Set (UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, GetDepth());
						m_TapController.OnTap (m_Camera.ScreenToWorldPoint(m_PosBuffer));
						break;
				}
			}
		}
	}
}
