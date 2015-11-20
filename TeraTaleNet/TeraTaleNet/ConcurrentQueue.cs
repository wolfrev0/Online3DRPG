using System.Collections.Generic;

namespace TeraTaleNet
{
    public class ConcurrentQueue<T>
    {
        Queue<T> _queue = new Queue<T>();
        object _lock = new object();

        public int Count { get { return _queue.Count; } }

        public void Enqueue(T item)
        {
            lock (_lock)
            {
                _queue.Enqueue(item);
            }
        }

        public T Dequeue()
        {
            lock (_lock)
            {
                return _queue.Dequeue();
            }
        }
    }
}
