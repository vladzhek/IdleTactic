using System;
using Reactive;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Models;
using Slime.Data.IDs;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using Utils;

namespace Slime.UI
{
    public class HealthViewModel : ViewModel
    {
        public event Action<Vector3, float> DamageDone;

        private IUnitsModel _unitsModel;
        public ReactiveDictionary<IUnit, HealthValue> UnitsHealth { get; private set; }

        private HealthViewModel(IUnitsModel unitsModel)
        {
            _unitsModel = unitsModel;
            UnitsHealth = new ReactiveDictionary<IUnit, HealthValue>();
        }

        public override void OnSubscribe()
        {
            foreach (var unit in _unitsModel.Units.Collection)
            {
                SubscribeForUnitChange(unit);
            }

            _unitsModel.Units.ItemAdded += OnUnitAdded;
            _unitsModel.Units.ItemRemoved += OnUnitRemoved;
        }

        public override void OnUnsubscribe()
        {
            foreach (var unit in _unitsModel.Units.Collection)
            {
                UnsubscribeForUnitChange(unit);
            }

            _unitsModel.Units.ItemAdded -= OnUnitAdded;
            _unitsModel.Units.ItemRemoved -= OnUnitRemoved;
        }

        public override void OnDispose()
        {
            _unitsModel = null;
            UnitsHealth = null;
        }

        private void OnUnitAdded(int index, IUnit unit)
        {
            SubscribeForUnitChange(unit);
        }

        private void OnUnitRemoved(IUnit unit)
        {
            if (UnitsHealth.Dictionary.ContainsKey(unit))
            {
                UnitsHealth.RemoveKey(unit);
            }

            UnsubscribeForUnitChange(unit);
        }

        private void SubscribeForUnitChange(IUnit unit)
        {
            unit.HealthChanged += OnHealthChanged;
        }

        private void UnsubscribeForUnitChange(IUnit unit)
        {
            unit.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(IUnit unit, float delta)
        {
            UnitsHealth.Dictionary.TryGetValue(unit, out var healthValue);
            UpdateHealthValue(unit, ref healthValue);
            UnitsHealth.SetItem(unit, healthValue);

            DamageDone?.Invoke(unit.Position, delta);
        }

        private void UpdateHealthValue(IUnit unit, ref HealthValue healthValue)
        {
            var health = unit.GetParameter(AttributeIDs.Health);
            healthValue.Current = health.Value;
            healthValue.Max = health.Attribute.FinalValue;
            healthValue.Unit = unit;
        }
    }
}