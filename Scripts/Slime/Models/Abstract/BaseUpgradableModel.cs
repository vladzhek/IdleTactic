using System;
using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer.Equipment;
using Slime.AbstractLayer.Models;
using Slime.Configs.Abstract;
using Slime.Data.Abstract;
using Slime.Data.Progress.Abstract;
using UnityEngine;
using Utils.Extensions;
using Logger = Utils.Logger;

namespace Slime.Models.Abstract
{
    // TODO: make inherit BaseDisplayableModel
    public abstract class BaseUpgradableModel<TConfigs, TConfig, TData> : 
        BaseProgressModel,
        IUpgradableModel<TData>
        where TConfigs : ItemsConfig<TConfig, TData>
        where TConfig : ItemConfig<TData>
        where TData : class, IUpgradable, IClonable
    {
        // state
        private IEnumerable<TData> _configItems;
        private readonly Dictionary<string, TData> _equipment = new();

        protected BaseUpgradableModel(IGameProgressModel progressModel) : base(progressModel)
        {
        }

        protected virtual void OnSubscribe()
        {
        }
        
        protected virtual void OnUnsubscribe()
        {
        }
        
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

        protected abstract string ConfigPath { get; }

        protected virtual IEnumerable<TData> ProcessConfig(TConfigs config)
        {
            return config.Items;
        }
        
        protected virtual TData PrepareData(TData source)
        {
            if (source == null)
            {
                var message = $"type: {GetType()}; source can't be null"; 
                Logger.Error(message);
                throw new NullReferenceException(message);
            }
            
            //Logger.Log($"type: {source.GetType()}; id: {source.ID}");

            if (source.Clone() is not TData result)
            {
                throw new Exception($"type: {GetType()}; can't clone {source.ID}");
            }

            var progress = GetProgressData(result.ID);
            //Logger.Log($"progress: {progress}");
            if (progress != null) result.SetProgress(progress);
            return result;
        }
        
        protected virtual void UpdateState(string id = null)
        {
            //Logger.Log($"type: {GetType()}; id: {id}");
            if (GameData == null)
            {
                Logger.Warning($"type: {GetType()}; no game data");
                return;
            }

            if (ConfigItems == null)
            {
                Logger.Warning($"type: {GetType()}; no config");
                return;
            }
            
            var items = from item in ConfigItems 
                where string.IsNullOrEmpty(id) || item.ID == id
                select PrepareData(item);
            foreach (var item in items)
            {
                // TODO: equatable interface to avoid unnecessary changes?
                _equipment[item.ID] = item;
                TriggerOnItemChange(item);
            }
            
            TriggerOnChange();
        }

        protected  void TriggerOnItemChange(TData data)
        {
            OnItemChange?.Invoke(data);
        }

        protected void TriggerOnChange()
        {
            OnChange?.Invoke();
        }

        protected void TriggerOnUpgrade(TData data)
        {
            OnUpgrade?.Invoke(data);
        }

        #region Progress
        
        protected override void OnProgressLoaded()
        {
            base.OnProgressLoaded();
            
            UpdateState();
        }

        protected abstract Dictionary<string, ProgressData> GetProgressData();

        protected ProgressData GetProgressData(string id, bool autoInsert = false)
        {
            if (string.IsNullOrEmpty(id))
            {
                Logger.Error($"model: {GetType()}");
                throw new Exception("id cannot be null");
            }
            
            var progressData = GetProgressData();
            var data = progressData.GetValueOrDefault(id);
            if (autoInsert && data == null)
            {
                data = new ProgressData();
                progressData.Add(id, data);
            }
            
            return data;
        }
        
        #endregion

        #region IInitializable implementation
        
        public override void Initialize()
        {
            base.Initialize();
            
            OnSubscribe();
        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            OnUnsubscribe();
        }

        #endregion
        
        #region IUpgradableModel implementation

        public event Action OnChange;
        public event Action<TData> OnItemChange;

        public virtual IEnumerable<TData> Get()
        {
            return _equipment.Values;
        }

        public TData Get(string id)
        {
            return string.IsNullOrEmpty(id) ? null : _equipment.GetValueOrDefault(id);
        }

        public event Action<TData> OnUpgrade;

        public void Add(string id, int quantity = 1)
        {
            var progressData = GetProgressData(id, true);
            progressData.Quantity ??= 0;
            progressData.Quantity += quantity;
            
            if (progressData.Level is null or 0)
            {
                progressData.Level = 1;
            }

            var item = Get(id);
            item.SetProgress(progressData);
            
            TriggerOnItemChange(item);
            TriggerOnChange();
        }

        public virtual void Upgrade(string id)
        {
            // check that is unlocked
            var item = Get(id);
            if (!item.CanUpgrade)
            {
                Logger.Warning($"item {id} can't be upgraded (isUnlocked: {item.IsUnlocked}; progress {item.Progress})");
                return;
            }

            // calculate internal level/quantity
            item.Upgrade();
            
            // update progress data
            var progressData = GetProgressData(id, true); 
            new ProgressData // unnecessary bravado
            {
                Level = item.Level,
                Quantity = item.Quantity
            }.CopyFieldsTo(progressData);
            
            TriggerOnUpgrade(item);
            TriggerOnItemChange(item);
            TriggerOnChange();
        }

        public void UpgradeAll()
        {
            var upgradableItems = from item in Get() where item.CanUpgrade select item.ID;
            foreach (var id in upgradableItems)
            {
                Upgrade(id);
            }
            
            // TODO: display results (same as summon results screen)
        }

        #endregion
    }
}