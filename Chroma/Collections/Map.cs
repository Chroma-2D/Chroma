using System.Collections;
using System.Collections.Generic;

namespace Chroma.Collections
{
    public class Map<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        private readonly Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private readonly Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public Indexer<T1, T2> Forward { get; private set; }
        public Indexer<T2, T1> Reverse { get; private set; }

        public Map()
        {
            Forward = new Indexer<T1, T2>(_forward);
            Reverse = new Indexer<T2, T1>(_reverse);
        }

        public void Add(T1 t1, T2 t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        public void RemoveUsingKey(T1 key)
        {
            _reverse.Remove(_forward[key]);
            _forward.Remove(key);
        }

        public void RemoveUsingValue(T2 value)
        {
            _forward.Remove(_reverse[value]);
            _reverse.Remove(value);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => _forward.GetEnumerator();

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
            => _forward.GetEnumerator();

        public class Indexer<T3, T4>
        {
            private readonly Dictionary<T3, T4> _dictionary;

            public T4 this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }

            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }

            public bool Contains(T3 key)
                => _dictionary.ContainsKey(key);
        }
    }
}
