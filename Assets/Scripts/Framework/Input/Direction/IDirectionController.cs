using UnityEngine;

namespace Framework.Input.Direction
{
    public interface IDirectionController
    {
        void OnStartMove();
        void OnMove(Vector3 _Offset);
        void OnEndMove();
    }
}
