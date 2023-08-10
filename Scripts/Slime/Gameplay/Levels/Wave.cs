using System;
using System.Collections.Generic;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Services;
using Slime.Data.IDs;
using Utils.Time;

namespace Slime.Levels
{
    public class Wave : IWave
    {
        public event Action<IWave> Completed;
        public event Action<IWave> Failed;
        public event Action<string> UnitDied;

        public List<IUnit> Units { get; }
        public Timer Timer { get; }
        public bool IsActive { get; private set; }

        public Wave(List<IUnit> units, Timer timer = null)
        {
            Units = units;

            foreach (var unit in Units)
            {
                unit.Die += OnUnitDies;
            }

            if (timer != null)
            {
                Timer = timer;
                Timer.OnComplete += OnTimerCompleted;
            }
        }

        private void OnUnitDies(IUnit unit)
        {
            UnitDied?.Invoke(unit.ID);

            unit.Die -= OnUnitDies;
            Units.Remove(unit);

            if (Units.Count == 0)
            {
                Completed?.Invoke(this);
            }
        }

        private void OnTimerCompleted(ITimer _)
        {
            if (IsActive)
            {
                IsActive = false;
            }

            if (Units.Count > 0)
            {
                Failed?.Invoke(this);
            }

            Timer.OnComplete -= OnTimerCompleted;
        }

        public void Dispose()
        {
            if (Timer != null)
            {
                Timer.OnComplete -= OnTimerCompleted;
            }

            foreach (var unit in Units)
            {
                unit.Die -= OnUnitDies;
            }

            Units.Clear();
        }
    }
}