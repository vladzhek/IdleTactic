using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Slime.Configs.Abstract
{
    public abstract class ItemsConfig<TConfig, TData> : ScriptableObject
    where TConfig : ItemConfig<TData>
    {
        [SerializeField, AssetSelector] protected TConfig[] _items;
        
        public virtual IEnumerable<TData> Items => from config in _items select config.Item;
    }
}