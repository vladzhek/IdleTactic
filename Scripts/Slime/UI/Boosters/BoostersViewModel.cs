using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using UI.Base.MVVM;
using Utils.Time;

namespace Slime.UI.Boosters
{
    public class BoostersViewModel : ViewModel
    {
        private readonly UIManager _uiManager;
        private readonly IBoostersModel _boostersModel;
        private readonly TimerService _timerService;

        public BoostersViewModel(UIManager uiManager, IBoostersModel boostersModel, TimerService timerService)
        {
            _uiManager = uiManager;
            _boostersModel = boostersModel;
            _timerService = timerService;
        }

        public event Action OnChange;
        
        public event Action<string> OnActivated
        {
            add => _boostersModel.Activated += value;
            remove => _boostersModel.Activated -= value;
        }

        public event Action<string> OnDeactivated
        {
            add => _boostersModel.Deactivated += value;
            remove => _boostersModel.Deactivated -= value;
        }
 
        public Dictionary<string, BoosterLayoutElementViewData> Data {
            get
            {
                return (from booster in _boostersModel.Boosters.Values 
                    let data = booster?.Data 
                    let progress = booster?.Progress 
                    where data != null && progress != null 
                    select data).ToDictionary(data => data.ID, data => Get(data.ID));
            }
        }

        public void OnSelected(BoosterLayoutElementViewData viewData)
        {
            _boostersModel.ActivateBoosterForAds(viewData.ID);
        }
        
        public ITimer GetTimer(string id)
        {
            return _timerService.Timers[id];
        }
        
        public BoosterLayoutElementViewData Get(string id)
        {
            var booster = _boostersModel.Boosters[id];
            var data = booster?.Data;
            var progress = booster?.Progress;

            if (data == null || progress == null)
            {
                return null;
            }

            return new BoosterLayoutElementViewData(data.ID)
            {
                Amount = progress.Amount,
                BoosterSprite = data.Sprite,
                CurrentLvl = progress.Level,
                TotalCounts = data.ForNextUpgrade,
                ColorBG = data.ColorBG,
                Description = data.Description,
                Title = data.Title,
                Value = progress.BoosterEffectValue * 100,
                IsActive = progress.IsActive,
                Duration = data.Duration,
            };
        }

        public override void OnEnable()
        {
            base.OnEnable();

            _boostersModel.OnChange += OnBoostersChanged;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            _boostersModel.OnChange -= OnBoostersChanged;
        }

        private void OnBoostersChanged()
        {
            OnChange?.Invoke();
        }

        public void OnCloseButtonClicked()
        {
            _uiManager.Close<BoostersView>();
        }
    }
}