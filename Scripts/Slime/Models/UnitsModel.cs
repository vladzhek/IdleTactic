using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Reactive;
using Services;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Models;
using Utils;
using Utils.UpdateLoops;

namespace Slime.Models
{
    [UsedImplicitly]
    public class UnitsModel : IUnitsModel, IUpdateInLoop
    {
        public event Action<IUnit> UnitDied;
        public event Action<IUnit> OnEnemyDied;

        private readonly List<IUnit> _deadUnits = new List<IUnit>();
        private UpdateLoopsService _updateLoopsService;

        public EUpdateLoop UpdateLoop => EUpdateLoop.PostUpdate;
        public bool IsActive => true;
        public ReactiveCollection<IUnit> Units { get; } = new ReactiveCollection<IUnit>();
        public ReactiveCollection<IUnit> EnemyUnits { get; } = new ReactiveCollection<IUnit>();

        public UnitsModel(UpdateLoopsService updateLoopsService)
        {
            _updateLoopsService = updateLoopsService;
            _updateLoopsService.RegisterForUpdate(this);
        }

        public void OnUnitCreated(IUnit unit)
        {
            if (unit.Side == ESide.Enemy)
            {
                EnemyUnits.Add(unit);
            }

            if (unit.Avatar == null)
            {
                Logger.Error("Empty unit");
            }

            Units.Add(unit);
            unit.Die += OnUnitDies;
        }

        public void ClearEnemyUnits()
        {
            Logger.Log($"сlear {EnemyUnits.Collection.Count} units");
            
            foreach (var enemyUnit in EnemyUnits.Collection)
            {
                enemyUnit.Die -= OnUnitDies;
                _deadUnits.Add(enemyUnit);
            }
            
            ClearDeadUnits();
        }

        public void Update(float deltaTime)
        {
            ClearDeadUnits();
        }

        private void ClearDeadUnits()
        {
            foreach (var deadUnit in _deadUnits)
            {
                if (deadUnit == null)
                {
                    Logger.Warning($"Unit already disposed");
                }

                if (deadUnit.Side == ESide.Enemy)
                {
                    EnemyUnits.RemoveItem(deadUnit);
                    Units.RemoveItem(deadUnit);
                    deadUnit.Dispose();
                }
            }

            _deadUnits.Clear();
        }

        private void OnUnitDies(IUnit unit)
        {
            UnitDied?.Invoke(unit);

            if (unit.Side == ESide.Enemy)
            {
                OnEnemyDied?.Invoke(unit);
            }

            _deadUnits.Add(unit);

            unit.Die -= OnUnitDies;
        }
    }
}