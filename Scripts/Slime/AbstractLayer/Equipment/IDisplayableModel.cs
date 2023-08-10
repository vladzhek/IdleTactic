using System;
using System.Collections.Generic;

namespace Slime.AbstractLayer.Equipment
{
    public interface IDisplayableModel<out TData>
    {
        public event Action OnChange;
        public event Action<TData> OnItemChange;
        
        public IEnumerable<TData> Get();
        public TData Get(string id);
        public int IndexOf(string id);
    }
}