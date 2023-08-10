using System;
using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer.Equipment;
using Slime.Configs.Abstract;
using Slime.Data.Abstract;
using UnityEngine;
using Zenject;
using Logger = Utils.Logger;

namespace Slime.Models.Abstract
{
    public abstract class BaseDisplayableModel<TConfigs, TConfig, TData> :
        IDisplayableModel<TData>, 
        IInitializable, IDisposable
        where TConfigs : ItemsConfig<TConfig, TData>
        where TConfig : ItemConfig<TData>
        where TData : class, IClonable, IData
    {
        // state
        private IEnumerable<TData> _configItems;
        private List<string> _keys;
        private List<TData> _values;

        protected abstract string ConfigPath { get; }
        
        protected virtual IEnumerable<TData> ConfigItems => _configItems ??= LoadConfigItems();

        protected virtual IEnumerable<TData> LoadConfigItems()
        {
            // TODO: move to separate service
            var path = $"Configs/Version1/{ConfigPath}";
            var config = Resources.Load<TConfigs>(path);
            if (!config)
            {
                throw new Exception($"no config at {path}");
            }

            return ProcessConfig(config);
        }

        protected virtual IEnumerable<TData> ProcessConfig(TConfigs config)
        {
            return config.Items;
        }
        
        protected virtual TData PrepareData(TData source)
        {
            //Logger.Log($"type: {source.GetType()}; id: {source.ID}");

            if (source.Clone() is not TData result)
            {
                throw new Exception($"can't clone {source.ID}");
            }

            return result;
        }
        
        protected virtual void UpdateState(string id = null)
        {
            _keys ??= new List<string>();
            _values ??= new List<TData>();
            
            _keys.Clear();
            _values.Clear();
            
            var items = from item in ConfigItems 
                where string.IsNullOrEmpty(id) || item.ID == id
                select PrepareData(item);
            foreach (var item in items)
            {
                // TODO: equatable interface to avoid unnecessary changes?
                _keys.Add(item.ID);
                _values.Add(item);
                TriggerOnItemChange(item);
            }

            TriggerOnChange();
        }

        protected virtual void TriggerOnItemChange(TData data)
        {
            OnItemChange?.Invoke(data);
        }

        protected virtual void TriggerOnChange()
        {
            OnChange?.Invoke();
        }

        #region IInitializable implementation
        
        public virtual void Initialize()
        {
        }
        
        public virtual void Dispose()
        {
            _configItems = null;
            _keys = null;
            _values = null;
        }

        #endregion
        
        #region IDisplayableModel implementation

        public event Action OnChange;
        public event Action<TData> OnItemChange;

        public virtual IEnumerable<TData> Get()
        {
            if (_values == null)
            {
                UpdateState();
            }
            
            return _values;
        }

        public TData Get(string id)
        {
            return string.IsNullOrEmpty(id) ? null : _values[IndexOf(id)];
        }

        public int IndexOf(string id)
        {
            return _keys.IndexOf(id);
        }

        #endregion
    }
}