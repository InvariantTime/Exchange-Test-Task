using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Exchange.Presentation.Utils
{
    public class ReadOnlyObservableDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, INotifyCollectionChanged where TKey : notnull
    {
        private readonly ObservableDictionary<TKey, TValue> _base;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public ReadOnlyObservableDictionary(ObservableDictionary<TKey, TValue> dictionary)
        {
            _base = dictionary;
            _base.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
        }

        public TValue this[TKey key] => _base[key];

        public IEnumerable<TKey> Keys => _base.Keys;

        public IEnumerable<TValue> Values => _base.Values;

        public int Count => _base.Count;


        public bool ContainsKey(TKey key)
        {
            return _base.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _base.GetEnumerator();
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return _base.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _base.GetEnumerator();
        }

        private void OnCollectionChanged(object? _, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
}
