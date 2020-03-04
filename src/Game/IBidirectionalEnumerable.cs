using System.Collections.Generic;

namespace Game
{
    public interface IBidirectionalEnumerable<T> : IEnumerable<T>
    {
        IList<T> Items { get; }
        bool Remove(T item);
        IBidirectionalEnumerator<T> GetEnumerator();
    }

}
