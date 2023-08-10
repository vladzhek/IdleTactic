using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Base.Widgets
{
    public interface IWidget<out TElement, TData> 
        where TElement : LayoutElement<TElement, TData>
    {
        public event Action<TData> OnSelect;

        TElement LayoutWidgetElementPrefab { get; }
        RectTransform Container { get; }

        public void SetData(IEnumerable<TData> data);
        public void Clear();

        public TData[] GetData { get; }
        public void SetData(int index, TData data);
        
        public void SetSelectable(int index, bool isSelectable);
    }
}