using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Exchange.Presentation.Utils
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue> _base = new();

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public TValue this[TKey key] 
        {
            get => _base[key]; 
            
            set => _base[key] = value;
        }

        public ICollection<TKey> Keys => _base.Keys;

        public ICollection<TValue> Values => _base.Values;

        public int Count => _base.Count;

        public bool IsReadOnly => false;

        public void NotifyChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Add(TKey key, TValue value)
        {
            var valuePair = new KeyValuePair<TKey, TValue>(key, value);
            Add(valuePair);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)_base).Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            _base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _base.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _base.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_base).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _base.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            bool result = _base.Remove(key);

            if (result == true)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, key));

            return result;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool result = ((IDictionary<TKey, TValue>)_base).Remove(item);

            if (result == true)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));

            return result;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return _base.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _base.GetEnumerator();
        }
    }
}
