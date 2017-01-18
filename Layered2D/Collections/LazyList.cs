using System;
using System.Collections.Generic;

namespace Layered2D.Collections
{
    public class LazyList<T> : List<T>, ILazyList<T>
    {
        protected Queue<Tuple<T, int>> addQueue;
        protected Queue<T> removeQueue;

        public LazyList()
        {
            addQueue = new Queue<Tuple<T, int>>();
            removeQueue = new Queue<T>();
        }

        public virtual void LazyAdd(T item)
        {
            addQueue.Enqueue(new Tuple<T, int>(item, -1));
        }

        public virtual void LazyRemove(T item)
        {
            removeQueue.Enqueue(item);
        }

        public virtual void LazyInsert(int index, T item)
        {
            addQueue.Enqueue(new Tuple<T, int>(item, index));
        }

        public virtual bool Apply()
        {
            var result = (removeQueue.Count + addQueue.Count > 0);

            while (addQueue.Count > 0)
            {
                var itm = addQueue.Dequeue();

                if (itm.Item2 == -1)
                    base.Add(itm.Item1);
                else
                    base.Insert(itm.Item2, itm.Item1);
            }

            while (removeQueue.Count > 0)
                base.Remove(removeQueue.Dequeue());

            return result;
        }
    }
}
