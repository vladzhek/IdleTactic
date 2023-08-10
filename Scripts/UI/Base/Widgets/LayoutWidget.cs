using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Base.Widgets
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class LayoutWidget<TElementWidget, TData> : 
        MonoBehaviour, 
        IWidget<TElementWidget, TData> where TData : IEquatable<TData>
        where TElementWidget :  LayoutElement<TElementWidget, TData>
    {
        [SerializeField] protected TElementWidget _elementPrefab;
        [SerializeField] protected RectTransform _elementsContainer;

        public readonly List<TElementWidget> Elements = new();

        private int ElementsCount => Elements.Count;
        
        protected virtual void Awake()
        {
            if (_elementsContainer == null)
            {
                _elementsContainer = gameObject.GetComponent<RectTransform>();
            }
        }

        private void OnDestroy()
        {
            Clear();
        }

        protected virtual void Subscribe(TElementWidget element)
        {
            element.OnSelect += OnElementSelect;
        }
        
        protected virtual void Unsubscribe(TElementWidget element)
        {
            element.OnSelect -= OnElementSelect;
        }

        private void AddElement(TData data)
        {
            var element = Instantiate(_elementPrefab, _elementsContainer);
            element.SetIndex(Elements.Count);
            element.SetData(data);

            Elements.Add(element);
            
            Subscribe(element);
        }
        
        protected abstract void OnElementSelect(TElementWidget layoutWidgetElement);

        protected void OnSelected(TData data)
        {
            OnSelect?.Invoke(data);
        }
        
        #region IWidget implementation
        
        public event Action<TData> OnSelect;
        public TElementWidget LayoutWidgetElementPrefab => _elementPrefab;
        public RectTransform Container => _elementsContainer;
        
        public void SetData(IEnumerable<TData> data)
        {
            if (data == null)
            {
                return;
            }

            var dataArray = data as TData[] ?? data.ToArray();
            if (ElementsCount > dataArray.Length)
            {
                Clear();
            }
            
            for (var i = 0; i < dataArray.Length; i++)
            {
                if (i < Elements.Count)
                {
                    SetData(i, dataArray[i]);
                }
                else
                {
                    AddElement(dataArray[i]);
                }
            }
        }
        
        public virtual void Clear()
        {
            foreach (var element in Elements)
            {
                Unsubscribe(element);
                if (element && element.gameObject) {
                    Destroy(element.gameObject);
                }
            }

            Elements.Clear();

            var transform = _elementsContainer.transform;
            var count = transform.childCount;
            for (var i = count - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i).gameObject;
                Destroy(child);
            }
        }
        
        public TData[] GetData => Elements.Select(x => x.Data).ToArray();
        
        public void SetData(int index, TData data)
        {
            if (Elements.Count <= index)
            {
                for (var i = 0; i <= index; i++)
                {
                    AddElement(data);
                }
            }
            else
            {
                Elements[index].SetData(data);
            }
        }
        
        public void SetSelectable(int index, bool isSelectable)
        {
            if (index < 0 || index >= Elements.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            Elements[index].SetSelectable(isSelectable);
        }
        
        #endregion
    }
}