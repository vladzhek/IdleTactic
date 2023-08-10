using System;
using UI.Base.Widgets;

namespace Slime.UI.Abstract
{
    public abstract class BaseLayoutWidget<TElement,TData> : 
        LayoutWidget<TElement, TData> 
        where TElement : LayoutElement<TElement, TData>
        where TData : LayoutData<TData>, IEquatable<TData>
    {
        protected override void OnElementSelect(TElement element)
        {
            OnSelected(element.Data);
        }
    }
}