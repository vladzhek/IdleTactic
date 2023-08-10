using System;
using System.Collections.Generic;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Data.Enums;
using Slime.Data.IDs;
using UI.Base.MVVM;
using Utils;
using Utils.Extensions;

namespace Slime.UI.Store.Summon
{
    public class SummonViewModel : ViewModel
    {
        // dependencies
        private readonly ISummonModel _summonModel;
        private readonly ISummonResultUIModel _summonResultUIModel;
        private readonly ISummonInfoUIModel _summonInfoUIModel;
        private readonly IResourcesModel _resourcesModel;
        private readonly ITutorialModel _tutorialModel;
        private readonly IAdsService _adsService;
        private readonly ITimeTrackerService _timeTrackerService;

        // state
        private readonly Dictionary<ESummonType, SummonLayoutElementData> _data = new ();

        private SummonViewModel(
            ISummonModel summonModel,
            ISummonResultUIModel summonResultUIModel,
            ISummonInfoUIModel summonInfoUIModel,
            IResourcesModel resourcesModel,
            ITutorialModel tutorialModel,
            IAdsService adsService,
            ITimeTrackerService timeTrackerService)
        {
            _summonModel = summonModel;
            _summonResultUIModel = summonResultUIModel;
            _summonInfoUIModel = summonInfoUIModel;
            _resourcesModel = resourcesModel;
            _tutorialModel = tutorialModel;
            _adsService = adsService;
            _timeTrackerService = timeTrackerService;
        }
        
        public Dictionary<ESummonType, SummonLayoutElementData> Data => _data;

        public event Action<ESummonType, SummonLayoutElementData> OnChange;
        public event Action<string> OnTimerChange;

        // TODO: ViewModel should not have state as it is not updated when view is not enabled

        #region ViewModel overrides
        
        public override void OnEnable()
        {
            base.OnEnable();
            
            UpdateState();
        }

        public override void OnSubscribe()
        {
            _summonModel.OnItemChange += OnSummonItemChanged;
            _resourcesModel.OnChange += OnResourceChanged;
            _timeTrackerService.OnRefreshTimerTick += OnRefreshTimerTicked;
        }
        
        public override void OnUnsubscribe()
        {
            _summonModel.OnItemChange -= OnSummonItemChanged;
            _resourcesModel.OnChange -= OnResourceChanged;
            _timeTrackerService.OnRefreshTimerTick -= OnRefreshTimerTicked;
        }

        #endregion
        
        private void OnSummonItemChanged(ESummonType type)
        {
            UpdateState(type);
        }
        
        private void OnResourceChanged(EResource resource, float quantity)
        {
            if (resource == EResource.HardCurrency)
            {
                UpdateState();
            }
        }
        
        private void OnRefreshTimerTicked(int seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            OnTimerChange?.Invoke($"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}");
        }

        private void UpdateState()
        {
            foreach (var type in EnumExtensions<ESummonType>.Values)  
            {
                UpdateState(type);
            }
        }
        
        private void UpdateState(ESummonType type)
        {
            var (summonData, summonAdData) = _summonModel.Get(type);
            //Logger.Log($"summon: {summonData}");
            SetData(type, new SummonLayoutElementData(
                type,
                // TODO: move to model similar to attributes
                _resourcesModel.IsEnough(EResource.HardCurrency, summonData.LowSummonPrice),
                _resourcesModel.IsEnough(EResource.HardCurrency, summonData.HighSummonPrice),
                $"{summonData.LowSummonPrice}",
                $"{summonData.HighSummonPrice}",
                summonData.LowSummonQuantity,
                summonData.HighSummonQuantity,
                summonAdData.SummonQuantity, 
                summonAdData.MaxSummonQuantity, 
                summonAdData.RemainingViewsQuantity,
                summonAdData.MaxViewsQuantity,
                summonData.Level,
                (int)summonData.Quantity,
                summonData.UpgradeQuantity));
        }
        
        private void SetData(ESummonType type, SummonLayoutElementData data)
        {
            _data[type] = data;
            OnChange?.Invoke(type, data);
        }

        private async void Summon(ESummonType type, ESummonCategory category)
        {
            var (_, adData) = _summonModel.Get(type);
            if (category == ESummonCategory.Ad)
            {
                // check that you can summon
                if (adData.RemainingViewsQuantity < 1)
                {
                    // NOTE: popup maybe
                    return;
                }

                // show ad
                if (!await _adsService.ShowAd(AdPlacementIDs.SUMMON))
                {
                    Logger.Warning("ads not successful");
                    return;
                }
            }
            
            var items = _summonModel.Summon(type, category);
            _summonResultUIModel.Open(type, items);
        }
        
        #region View events
        
        public void OnInfoButtonClicked(ESummonType type)
        {
            _summonInfoUIModel.Open(type);
        }
        
        public void OnAdButtonClicked(ESummonType type)
        {
            Summon(type, ESummonCategory.Ad);
        }
        
        public void OnLowQuantitySummonButtonClicked(ESummonType type)
        {
            Summon(type, ESummonCategory.Low);
        }
        
        public void OnHighQuantitySummonButtonClicked(ESummonType type)
        {
            Summon(type, ESummonCategory.High);
        }

        public bool GetTutorialActive()
        {
            return _tutorialModel.Stage == ETutorialStage.Summon;
        }
        
        #endregion
    }
}