using System.Linq;
using ModestTree;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Models;
using Slime.Data.IDs;
using UnityEngine;

namespace Services
{
    public class AimService
    {
        private readonly IPlayer _player;
        private readonly IUnitsModel _unitsModel;

        public AimService(IPlayer player, IUnitsModel unitsModel)
        {
            _player = player;
            _unitsModel = unitsModel;
        }

        public IUnit GetClosestEnemyUnit(Vector3 position)
        {
            if (_unitsModel.Units.Collection.Count == 0 || _unitsModel.EnemyUnits.Collection.Count == 0)
            {
                return null;
            }

            var closestDistance = Mathf.Infinity;
            IUnit closestUnit = null;
            var aliveEnemies = _unitsModel.EnemyUnits.Collection
                .Where(x => x.IsActive && x.GetParameter(AttributeIDs.Health).PromisedValue > 0);
            foreach (var unit in aliveEnemies)
            {
                var distance = Vector3.Distance(unit.Position, position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestUnit = unit;
                }
            }

            return closestUnit;
        }

        public IUnit GetRandomEnemyUnit(Vector3 position)
        {
            if (_unitsModel.Units.Collection.Count == 0 || _unitsModel.EnemyUnits.Collection.Count == 0)
            {
                return null;
            }

            var aliveEnemies = _unitsModel.EnemyUnits.Collection
                .Where(x => x.IsActive && x.GetParameter(AttributeIDs.Health).PromisedValue > 0).ToArray();
            if (!aliveEnemies.Any())
            {
                return null;
            }

            var randomIndex = Random.Range(0, aliveEnemies.Length);
            return aliveEnemies[randomIndex];
        }

        public IUnit GetClosestToPlayerEnemyUnit()
        {
            if (_player.Unit == null)
            {
                return null;
            }

            return GetClosestEnemyUnit(_player.Unit.Position);
        }

        public IUnit[] GetUnitsInArea(Vector3 position, float radius)
        {
            return _unitsModel.EnemyUnits.Collection
                .Where(x => x.GetParameter(AttributeIDs.Health).PromisedValue > 0)
                .Where(x => Vector3.Distance(x.Position, position) < radius)
                .ToArray();
        }
    }
}