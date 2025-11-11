using UnityEngine;

namespace Framework.Draw
{
    public interface IDrawLine
    {
        void AddPoint(Vector3 _Pos, bool _IsSubBrush = false);
    }
}