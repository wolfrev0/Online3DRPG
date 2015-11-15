using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeraTaleNet
{
    public class ConcurrentQueue<T>
    {
        Queue<T> _queue = new Queue<T>();
        object _lockObject = new object();

        public int Count { get { return _queue.Count; } }

        public void Enqueue(T item)
        {
            lock (_lockObject)
            {
                _queue.Enqueue(item);
            }
        }

        public T Dequeue()
        {
            lock (_lockObject)
            {
                return _queue.Dequeue();
            }
        }
    }
}
