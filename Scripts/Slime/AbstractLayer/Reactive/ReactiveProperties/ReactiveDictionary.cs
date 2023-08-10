using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reactive
{
    public class ReactiveDictionary<TKey, TValue>
    {
        public event Action<ReactiveDictionary<TKey, TValue>> DictionaryChanged;
        public event Action<TKey, TValue> ItemChanged;
        public event Action<TKey, TValue, TValue> ItemReplaced;
        public event Action<TKey, TValue> ItemRemoved;
        public event Action<TKey, TValue> ItemAdded;

        private IDictionary<TKey, TValue> _dictionary;

        public IDictionary<TKey, TValue> Dictionary
        {
            get => _dictionary;
            set
            {
                _dictionary = value;
                DictionaryChanged?.Invoke(this);
            }
        }
        
        public ReactiveDictionary()
        {
            Dictionary = new Dictionary<TKey, TValue>();
        }
        
        public ReactiveDictionary(IDictionary<TKey, TValue> dictionary)
        {
            Dictionary = dictionary;
        }

        public void AddPair(TKey key, TValue value)
        {
            Dictionary.Add(key, value);
            ItemAdded?.Invoke(key, value);
            DictionaryChanged?.Invoke(this);
        }

        public void SetItem(TKey key, TValue newItem)
        {
            if (Dictionary.ContainsKey(key))
            {
                var previous = Dictionary[key];
                Dictionary[key] = newItem;
                if (!previous.Equals(newItem))
                {
                    ItemReplaced?.Invoke(key, previous, newItem);
                }
            }
            else
            {
                AddPair(key, newItem);
            }

            ItemChanged?.Invoke(key, newItem);
        }

        public void RemoveKey(TKey key)
        {
            if (!_dictionary.ContainsKey(key))
            {
                Debug.LogWarning($"Trying to remove key {key}, which is not in dictionary");
                return;
            }
            
            var item = Dictionary[key];
            ItemRemoved?.Invoke(key, item);
            Dictionary.Remove(key);
            DictionaryChanged?.Invoke(this);
        }
        
        public void RemoveItem(TKey key, TValue item)
        {
            Dictionary.Remove(new KeyValuePair<TKey, TValue>(key, item));
            DictionaryChanged?.Invoke(this);
        }

        public void ClearItems()
        {
            Dictionary.Clear();
            DictionaryChanged?.Invoke(this);
        }
    }
}