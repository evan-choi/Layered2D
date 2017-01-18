using System.Collections.Generic;

namespace Layered2D.Collections
{
    public interface ILazyList<T> : IList<T>
    {
        void LazyAdd(T item);
        void LazyRemove(T item);
        void LazyInsert(int index, T item);
        bool Apply();
    }
}
