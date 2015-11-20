using System.Collections.Generic;

namespace TeraTaleNet
{
    public class ConcurrentDictionary<TKey, TValue>
    {
        Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
        object _lock = new object();

        public void Add(TKey key, TValue value)
        {
            lock (_lock)
            {
                _dictionary.Add(key, value);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary[key];
                }
            }
            set
            {
                lock (_lock)
                {
                    _dictionary[key] = value;
                }
            }
        }

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary.Values;
                }
            }
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary.Keys;
                }
            }
        }

        public void Remove(TKey key)
        {
            _dictionary.Remove(key);
        }
    }
}
