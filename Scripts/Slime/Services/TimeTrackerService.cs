using System;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.Models.Abstract;
using UnityEngine;
using Utils.Time;

namespace Slime.Services
{
    // NOTE: this is not a model
    public class TimeTrackerService : BaseProgressModel, ITimeTrackerService
    {
        #region IInitializable implementation
        
        public override void Initialize()
        {
            base.Initialize();
            
            CreateRefreshTimer();
        }

        public override void Dispose()
        {
            base.Dispose();
            
            RemoveRefreshTimer();
        }
        
        #endregion
        
        #region ITimeTrackerService

        public event Action OnRefreshTimerComplete;
        public event Action<int> OnRefreshTimerTick;

        public bool IsNewDay()
        {
            // NOTE: it's not trackers responsibility but appropriate models
            // NOTE: get date of last session from SettingsModel
            // NOTE: getter should not have side effects
            // NOTE: you can't ask time directly - only through TimeService
            // NOTE: any resource models should track their resource state and refresh it idempotently
            // TODO: take a look at EntityRefresher
            //if (DateTime.Now.DayOfYear > GameData.DateOfLastSession.DayOfYear)
            if (_timeService.CurrentTime.DayOfYear > GameData.DateOfLastSession.DayOfYear)
            {
                GameData.AdsData?.Clear();

                return true;
            }

            return false;
        }

        public IEntityRefresher RequestRefresher(ERefreshableEntity entity)
        {
            return _entitiesRefresherFactory.Create(entity);
        }

        #endregion
        
        // private

        private readonly ITimeService _timeService;
        private readonly TimerService _timerService;
        private readonly DailyEntityRefresher.Factory _entitiesRefresherFactory;

        private ITimer _timer;
        
        private TimeTrackerService(IGameProgressModel progressModel,
            ITimeService timeService,
            TimerService timerService,
            DailyEntityRefresher.Factory entitiesRefresherFactory
            ) : base(progressModel)
        {
            _timeService = timeService;
            _timerService = timerService;
            _entitiesRefresherFactory = entitiesRefresherFactory;
        }

        private void OnTimerCompleted(ITimer _)
        {
            OnRefreshTimerComplete?.Invoke();
                
            RemoveRefreshTimer();
            CreateRefreshTimer();
        }

        private void OnTimerTicked(int seconds)
        {
            OnRefreshTimerTick?.Invoke(seconds);
        }

        private void CreateRefreshTimer()
        {
            var now = _timeService.CurrentTime;
            var endOfDay = now.Date.AddDays(1).AddSeconds(-1);
            var duration = Mathf.CeilToInt((float)(endOfDay - now).TotalSeconds);
            
            _timer = _timerService.CreateTimer(TimerIDs.REFRESH, duration);
            _timer.OnComplete += OnTimerCompleted;
            _timer.OnTick += OnTimerTicked;
        }

        private void RemoveRefreshTimer()
        {
            if (_timer != null)
            {
                _timer.OnComplete -= OnTimerCompleted;
                _timer.OnTick -= OnTimerTicked;
                _timer = null;
            }
            
            _timerService.RemoveTimer(TimerIDs.REFRESH);
        }
    }
}