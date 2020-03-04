using System.Collections.Generic;

namespace Game
{
    public interface IBidirectionalEnumerator<T> : IEnumerator<T>
    {
        bool MoveBack();
        bool Freeze();
    }
}
