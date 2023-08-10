using System;
using Reactive;
using Slime.AbstractLayer.Battle;

namespace Slime.AbstractLayer.Models
{
    public interface IUnitsModel
    {
        public event Action<IUnit> UnitDied;
        public event Action<IUnit> OnEnemyDied;
        public ReactiveCollection<IUnit> Units { get; }
        public ReactiveCollection<IUnit> EnemyUnits { get; }
        void OnUnitCreated(IUnit unit);
        void ClearEnemyUnits();
    }
}