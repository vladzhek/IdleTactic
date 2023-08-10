using System;
using System.Collections.Generic;

namespace Reactive
{
    public class ReactiveCollection<T>
    {
        public event Action<ReactiveCollection<T>> CollectionChanged;
        public event Action<int, T> ItemChanged;
        public event Action<int, T> ItemAdded;
        public event Action<T> ItemRemoved;

        private IList<T> _collection;

        public ReactiveCollection()
        {
            Collection = new List<T>();
        }

        public ReactiveCollection(IList<T> collection)
        {
            Collection = collection;
        }

        public T this[int index]
        {
            get { return _collection[index]; }
        }

        public IList<T> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                CollectionChanged?.Invoke(this);
            }
        }

        public void Add(T newItem)
        {
            Collection.Add(newItem);
            CollectionChanged?.Invoke(this);
            ItemAdded?.Invoke(Collection.Count - 1, newItem);
            ItemChanged?.Invoke(Collection.Count - 1, newItem);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                return;
            }

            var count = 0;
            foreach (var item in items)
            {
                if (Collection.Contains(item))
                {
                    continue;
                }

                Collection.Add(item);
                ItemChanged?.Invoke(Collection.Count - 1, item);
                count++;
            }

            if (count > 0)
            {
                CollectionChanged?.Invoke(this);
            }
        }

        public void SetItem(int index, T newItem)
        {
            Collection[index] = newItem;
            ItemChanged?.Invoke(index, newItem);
        }

        public void RemoveItem(int index)
        {
            var item = _collection[index];
            Collection.RemoveAt(index);
            ItemRemoved?.Invoke(item);
            CollectionChanged?.Invoke(this);
        }

        public void RemoveItem(T item)
        {
            Collection.Remove(item);
            ItemRemoved?.Invoke(item);
            CollectionChanged?.Invoke(this);
        }

        public void ClearItems()
        {
            Collection.Clear();
            CollectionChanged?.Invoke(this);
        }
    }
}