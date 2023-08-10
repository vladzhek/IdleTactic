using System;
using ModestTree;
using Slime.AbstractLayer.Models;
using Slime.Data.Boosters;
using Utils;
using Utils.Time;

namespace Slime.Models
{
    public abstract class Booster : IBooster
    {
        public event Action<string> OnDeactivate;

        public BoosterProgressData Progress { get; }
        public BoosterData Data { get; }
        public Timer Timer { get; set; }

        public float BlessingValue { get; set; } = 1;

        public Booster(BoosterProgressData progress, BoosterData data, TimerService timerService)
        {
            Progress = progress;
            this.Data = data;
            CreateTimer(timerService);
        }

        public virtual void Activate()
        {
            Progress.IsActive = true;
            BlessingValue += Progress.BoosterEffectValue;
            Timer.Start();
            Timer.OnComplete += OnTimerCompleted;
        }

        public virtual void Deactivate()
        {
            Progress.IsActive = false;
            BlessingValue -= Progress.BoosterEffectValue;
            Timer.OnComplete -= OnTimerCompleted;
            Timer.SetDuration(Data.Duration);
            
            OnDeactivate?.Invoke(Data.ID);
        }

        public virtual void Upgrade()
        {
            if (Progress.Level >= Data.MaxLevel)
            {
                Log.Debug($"Booster {Data.ID} достиг максимального уровня");
                return;
            }

            if (Progress.Amount >= Data.ForNextUpgrade)
            {
                Progress.Amount -= Data.ForNextUpgrade;
                Progress.Level++;
                Progress.BoosterEffectValue += Data.IncreaseValueForLevel;
            }
        }

        private void CreateTimer(TimerService timerService)
        {
            var timeToDeactivation = Progress.TimeToDeactivation > 0
                ? Progress.TimeToDeactivation
                : FormatTime.MinutesIntFormat(Data.Duration);

            Timer = timerService.CreateTimer(Data.ID, timeToDeactivation);

            Timer.Pause();
        }

        private void OnTimerCompleted(ITimer _)
        {
            Deactivate();
        }
    }
}