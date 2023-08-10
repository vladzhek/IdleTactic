using System;
using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer.Equipment;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Configs.Summon;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using Slime.Data.Progress.Abstract;
using Slime.Data.Summon;
using Slime.Exceptions;
using Slime.Models.Abstract;
using Utils;
using Utils.Extensions;

namespace Slime.Models
{
    public class SummonModel : 
        BaseUpgradableModel<SummonConfig, SummonItemConfig, SummonBaseData>,
        IAuthorizedResourceUser,
        ISummonModel
    {
        public string AuthorizationToken => Data.Constants.System.RESOURCE_TOKEN;
        
        private readonly IResourcesModel _resourcesModel;
        private readonly IInventoryModel _inventoryModel;
        private readonly ISkillsModel _skillsModel;
        private readonly ICompanionsModel _companionsModel;
        private readonly ITimeTrackerService _timeTrackerService;
        
        public SummonModel(IGameProgressModel progressModel,
            IResourcesModel resourcesModel,
            IInventoryModel inventoryModel,
            ISkillsModel skillsModel,
            ICompanionsModel companionsModel,
            ITimeTrackerService timeTrackerService
            ) : base(progressModel)
        {
            _resourcesModel = resourcesModel;
            _inventoryModel = inventoryModel;
            _skillsModel = skillsModel;
            _companionsModel = companionsModel;
            _timeTrackerService = timeTrackerService;
        }
        
        #region BaseUpgradableModel overrides

        protected override void OnProgressLoaded()
        {
            base.OnProgressLoaded();

            CheckAdsProgress();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            
            base.OnItemChange += OnItemChanged;
            _timeTrackerService.OnRefreshTimerComplete += OnRefreshed;
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            base.OnItemChange -= OnItemChanged;
            _timeTrackerService.OnRefreshTimerComplete -= OnRefreshed;
        }

        protected override string ConfigPath => "Summon";

        protected override IEnumerable<SummonBaseData> ProcessConfig(SummonConfig config)
        {
            var summonData = config.Data;
            var summonAdData = config.AdData;
            if (summonData == null || summonAdData == null)
            {
                throw new Exception("no summon config");
            }
            
            var data = new List<SummonBaseData>();
            foreach (var type in EnumExtensions<ESummonType>.Values)
            {
                var summonDataItem = summonData.Clone() as SummonData;
                summonDataItem!.SetType(type);
                data.Add(summonDataItem);
                
                var summonAdDataItem = summonAdData.Clone() as SummonAdData;
                summonAdDataItem!.SetType(type);
                data.Add(summonAdDataItem);
            }
            
            return data;
        }
        
        protected override Dictionary<string, ProgressData> GetProgressData()
        {
            var data = GameData.SummonData;
            if (data == null)
            {
                data = new Dictionary<string, ProgressData>();
                GameData.SummonData = data;
            }

            return data;
        }
        
        #endregion

        #region ISummonModel implementation

        public new event Action<ESummonType> OnItemChange;

        public event Action<ESummonType> OnSummon;

        public (SummonData,SummonAdData) Get(ESummonType type)
        {
            SummonData summonData = null;
            SummonAdData summonAdData = null;
            foreach (var item in Get())
            {
                if (item.Type == type)
                {
                    if (item is SummonAdData adData)
                    {
                        summonAdData = adData;
                    } else if (item is SummonData data)
                    {
                        summonData = data;
                    }
                }
                
                if (summonData != null && summonAdData != null) break;
            }

            if (summonData == null || summonAdData == null) throw new Exception("no summon data");
            
            return (summonData, summonAdData);
        }

        public IEnumerable<ISummonable> Summon(ESummonType type, ESummonCategory category)
        {
            var (data, adData) = Get(type);
            
            // check if enough resources
            var resourceQuantity = category switch
            {
                ESummonCategory.Ad => 0,
                ESummonCategory.Low => data.LowSummonPrice,
                ESummonCategory.High => data.HighSummonPrice,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, $"unknown type {type}")
            };
            if (!_resourcesModel.TrySpend(this, EResource.HardCurrency, resourceQuantity))
            {
                throw new NotEnoughResourceException();
            }
            
            // determine quantity to summon
            var totalQuantity = category switch
            {
                ESummonCategory.Ad => adData.SummonQuantity,
                ESummonCategory.Low => data.LowSummonQuantity,
                ESummonCategory.High => data.HighSummonQuantity,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, $"unknown type {type}")
            };

            // generate quantity of rarities according to probability
            var rarityValues = ERarityExtensions.Implemented.ToArray();
            var probabilities = (from rarity in rarityValues select data.GetProbability(rarity)).ToArray();
            var rarities = new List<ERarity>();
            for (var i = 0; i < totalQuantity; i++)
            {
                var randomValue = UnityEngine.Random.value;
                var sum = 0f;
                
                for (var j = 0; j < probabilities.Length; j++)
                {
                    sum += probabilities[j];
                    if (randomValue <= sum)
                    {
                        rarities.Add(rarityValues[j]);
                        break;
                    }
                }
            }
            
            // model to process generation of item of rarity and progress
            if (type switch
                {
                    ESummonType.Gear => (object)_inventoryModel,
                    ESummonType.Skills => _skillsModel,
                    ESummonType.Companions => _companionsModel,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, $"unknown type {type}")
                } is not ISummonnableModel<ISummonable> model)
            {
                throw new Exception($"model can't process {type}");
            }
            
            // generate item of each rarity
            var items = (from rarity in rarities
                let item = model.GetRandomConfigItem(rarity)
                where item != null
                select item).ToArray();
            
            // get items of each rarity
            var quantities = from item in items
                group item by item.ID into itemsByID
                select new { ID = itemsByID.Key, Quantity = itemsByID.Count() };
            
            // add each ID quantity
            foreach (var item in quantities)
            {
                var id = item.ID;
                var quantity = item.Quantity;
                model.Add(id, quantity);
            }
            
            // upgrade ad
            if (category == ESummonCategory.Ad)
            {
                var adId = adData.ID;
                
                // upgrades for one level 
                adData.Upgrade();
                
                // save only level
                var progressData = GetProgressData(adId, true); 
                new ProgressData // unnecessary bravado
                {
                    Level = adData.Level,
                }.CopyFieldsTo(progressData);
                
                // adds one view and triggers change
                Add(adId);
            }
            
            // upgrade summon
            Add(data.ID, totalQuantity);
            if (data.CanUpgrade)
            {
                Upgrade(data.ID);
            }

            OnSummon?.Invoke(type);
            return items;
        }

        #endregion

        private void OnItemChanged(SummonBaseData baseData)
        {
            OnItemChange?.Invoke(baseData.Type);
        }

        private void OnRefreshed()
        {
            CheckAdsProgress();
        }

        private void CheckAdsProgress()
        {
            // if we need to refresh summonAds progress, we set all SummonAdData.Quantity to zero
            using (var refresher = _timeTrackerService.RequestRefresher(ERefreshableEntity.SummonAds))
            {
                Logger.Warning($"needs to refresh: {refresher.NeedsRefresh}");
                
                if (!refresher.NeedsRefresh) return;
            }

            var items = from item in Get() 
                    where item is SummonAdData
                    select item as SummonAdData;

            foreach (var item in items)
            {
                var progress = GetProgressData(item.ID);
                if (progress == null) continue;
                
                progress.Quantity = 0;
                item.SetProgress(progress);
                TriggerOnItemChange(item);
            }
                
            TriggerOnChange();
        }
    }
}