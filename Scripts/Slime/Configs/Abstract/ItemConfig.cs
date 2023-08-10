using UnityEngine;

namespace Slime.Configs.Abstract
{
    public abstract class ItemConfig<T> : ScriptableObject
    {
        [SerializeField] protected T _item;
        
        public virtual T Item => _item;
    }
}