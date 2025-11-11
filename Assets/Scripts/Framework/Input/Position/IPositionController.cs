using UnityEngine;

namespace Framework.Input.Position
{
	public interface IPositionController
	{
		void OnStartMove (Vector3 _Pos);
		void OnMove (Vector3 _Offset);
		void OnEndMove (Vector3 _Pos);
	}
}
