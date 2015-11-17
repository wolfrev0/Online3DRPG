using System.Collections.Generic;

namespace TeraTaleNet
{
    public class ConcurrentDictionary<TKey, TValue>
    {
        Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
        object _lockObject = new object();

        public void Add(TKey key, TValue value)
        {
            lock (_lockObject)
            {
                _dictionary.Add(key, value);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_lockObject)
                {
                    return _dictionary[key];
                }
            }
            set
            {
                lock (_lockObject)
                {
                    _dictionary[key] = value;
                }
            }
        }

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                lock (_lockObject)
                {
                    return _dictionary.Values;
                }
            }
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                lock (_lockObject)
                {
                    return _dictionary.Keys;
                }
            }
        }
    }
}
