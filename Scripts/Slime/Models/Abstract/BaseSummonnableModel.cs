using System;
using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer.Equipment;
using Slime.AbstractLayer.Models;
using Slime.Configs.Abstract;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using Utils.Extensions;
using Logger = Utils.Logger;

namespace Slime.Models.Abstract
{
    public abstract class BaseSummonnableModel<TConfigs, TConfig, TData, TKey> : 
        BaseUpgradableModel<TConfigs, TConfig, TData>, 
        ISummonnableModel<TData, TKey>,
        IEquipmentModel
        where TConfigs : ItemsConfig<TConfig, TData>
        where TConfig : ItemConfig<TData>
        where TData : class, ISummonable, new()
    {
        #region ISummonnableModel implementation

        public event Action<string, bool> OnEquip;

        public IEnumerable<TData> GetEquipped()
        {
            return from item in Get() where item.IsEquipped select item;
        }

        public TData GetEquipped(TKey type)
        {
            var id = GetEquippedData().GetValueOrDefault(type);
            return string.IsNullOrEmpty(id) ? null : Get(id);
        }

        public virtual void Equip(string id, bool shouldEquip = true)
        {
            //Logger.Log($"id: {ID}; should equip: {shouldEquip}");
            
            // check if already equipped
            var isAlreadyEquipped = IsEquipped(id);
            if (isAlreadyEquipped == shouldEquip)
            {
                Logger.Warning($"equipment {id} is already {(shouldEquip ? "equipped" : "unequipped")}");
                return;
            }
            
            // check that is unlocked
            var item = Get(id);
            if (!item.IsUnlocked)
            {
                Logger.Warning($"equipment {id} can't be {(shouldEquip ? "equipped" : "unequipped")} until unlocked");
                return;
            }

            // toggle equip state
            var equippedData = GetEquippedData();
            var key = GetEquipmentKey(item);
            if (shouldEquip)
            {
                equippedData[key] = item.ID;
            }
            else
            {
                equippedData.Remove(key);
            }
            
            UpdateState();
            OnEquip?.Invoke(id, shouldEquip);
        }

        public TData GetRandomConfigItem(ERarity rarity)
        {
            var data = (from item in ConfigItems where item.Rarity == rarity select item).GetRandom();
            if (data == null)
            {
                Logger.Error($"data is null, type: {GetType()} rarity: {rarity}");
            }
            else
            {
                // NOTE: could send to PrepareData, but we don't need progress
                data.Sprite = _spritesModel.Get(data.ID.TrimEndDigits());
            }
            
            return data;
        }
        
        #endregion

        #region IEqupmentModel implementation

        public IDictionary<EAttribute, float> ParameterModifiers
        {
            // TODO: for this to be prettified EntityData.ActiveValue should return ParameterData
            get
            {
                var result = new Dictionary<EAttribute, float>
                {
                    { EAttribute.Damage, 0 },
                    { EAttribute.Health, 0 }
                };

                foreach (var data in Get())
                {
                    if (!data.IsUnlocked)
                    {
                        continue;
                    }

                    if (ShouldUseActiveValueInParameterCalculations && IsEquipped(data.ID))
                    {
                        result[DataToParameter(data)] += data.ActiveValue;
                    }
                    
                    result[DataToParameter(data)] += data.PassiveValue;
                }

                return result;
            }
        }

        #endregion
        
        
        // dependencies
        private readonly ISpritesModel _spritesModel;
        
        protected BaseSummonnableModel(IGameProgressModel progressModel,
            ISpritesModel spritesModel) : base(progressModel)
        {
            _spritesModel = spritesModel;
        }

        protected override TData PrepareData(TData source)
        {
            var data = base.PrepareData(source);
            data.IsEquipped = IsEquipped(data.ID);
            data.Sprite = _spritesModel.Get(data.ID.TrimEndDigits());
            return data;
        }
        
        public override IEnumerable<TData> Get()
        {
            return from item in base.Get() 
                orderby item.Rarity, item.Order
                select item;
        }

        public int IndexOf(string id)
        {
            throw new NotImplementedException();
        }

        protected abstract Dictionary<TKey, string> GetEquippedData();

        protected abstract TKey GetEquipmentKey(TData data);

        protected virtual bool IsEquipped(string id)
        {
            return GetEquippedData().ContainsValue(id);
        }
        
        // TODO: store in EntityData
        protected virtual EAttribute DataToParameter(TData data)
        {
            return EAttribute.Damage;
        }

        protected virtual bool ShouldUseActiveValueInParameterCalculations => true;
    }
}