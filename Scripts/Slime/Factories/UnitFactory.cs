using System.Collections.Generic;
using System.Linq;
using Slime.Configs;
using Slime.Battle.Units;
using Slime.Behaviours;
using Data.Units;
using Pooling;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Configs;
using Slime.AbstractLayer.Models;
using Slime.Configs.Units;
using Slime.Data.IDs;
using Slime.Gameplay.Battle.Units;
using Slime.Gameplay.Behaviours;
using Slime.Services;
using UnityEngine;
using Utils.Pooling;
using Zenject;
using Attribute = Slime.AbstractLayer.Stats.Attribute;
using Logger = Utils.Logger;

namespace Slime.Factories
{
    public class UnitFactory
    {
        private readonly DiContainer _container;
        private readonly GameConfig _gameConfig;
        private readonly IUnitsModel _unitsModel;
        private readonly ISceneModel _sceneModel;

        private readonly Dictionary<string, UnityPool> _avatarsPool = new();
        
        public UnitFactory(DiContainer container, 
            GameConfig gameConfig,
            IUnitsModel unitsModel, 
            ISceneModel sceneModel)
        {
            _sceneModel = sceneModel;
            _unitsModel = unitsModel;
            _gameConfig = gameConfig;
            _container = container;
        }

        public IUnit CreateEnemy(string id, IUnitAvatar prefab, int level, IStageConfig upgradeConfig)
        {
            var unit = _container.Instantiate<Unit>(new[] { id });
            unit.InitializeParameters(GetAttributes(id));
            unit.InjectAvatar(GetAvatar(id, prefab));
            unit.SetBehaviour(_container.Instantiate<EnemyBehaviour>());
            var rotation = Quaternion.LookRotation(Vector3.back);
            unit.SetPosition(unit.Position, rotation);
            if (upgradeConfig.UpgradesConfig.TryGetValue(id, out var attributeUpgradesData))
            {
                foreach (var attributeUpgrades in attributeUpgradesData)
                {
                    var value = attributeUpgrades.Config.GetValueForLevel(level);
                    unit.GetAttribute(attributeUpgrades.AttributeID).SetValue(value);
                }
            }
            else
            {
                Logger.Error($"No upgrades data fot {id}");
            }

            _unitsModel.OnUnitCreated(unit);
            return unit;
        }

        public IUnit CreateCharacterUnit(string id)
        {
            var unit = _container.Instantiate<PlayerCharacterUnit>(new[] { id });
            // NOTE: this is units responsibility
            //unit.InitializeStats(GetAttributes(id));
            // NOTE: move this out to IPlayer
            unit.InjectAvatar(CreateCharacterAvatar(id));
            unit.SetBehaviour(_container.Instantiate<CharacterBehaviour>());
            var rotation = Quaternion.LookRotation(Vector3.forward);
            unit.SetPosition(unit.Position, rotation);
            _unitsModel.OnUnitCreated(unit);
            return unit;
        }

        public IUnit CreateCompanionUnit(string id, int index)
        {
            var unit = _container.Instantiate<CompanionUnit>(new[] { id });
            unit.SetIndex(index);
            unit.InitializeParameters(GetAttributes(id));
            unit.InjectAvatar(CreateCompanionAvatar(id));
            unit.SetBehaviour(_container.Instantiate<CompanionBehaviour>());
            var rotation = Quaternion.LookRotation(Vector3.forward);
            unit.SetPosition(unit.Position, rotation);
            _unitsModel.OnUnitCreated(unit);
            return unit;
        }

        public IUnitAvatar CreateCharacterAvatar(string id)
        {
            if (!_gameConfig.Characters.TryGetValue(id, out var prefab))
            {
                Logger.Error($"No avatar for {id} in player's units");
                return null;
            }

            return GetAvatar(id, prefab);
        }
        
        public IUnitAvatar CreateCompanionAvatar(string id)
        {
            if (!_gameConfig.Companions.TryGetValue(id, out var prefab))
            {
                Logger.Error($"No avatar for {id} in player's units");
                return null;
            }

            return GetAvatar(id, prefab);
        }

        private Dictionary<string, Attribute> GetAttributes(string id)
        {
            if (!_gameConfig.UnitAttributes.TryGetValue(id, out var attributes))
            {
                Logger.Error($"No parameters for {id}");
                return null;
            }

            return attributes.Attributes.ToDictionary(x => x.AttributesID, x => new Attribute(x.AttributesID, x.Value));
        }

        private IUnitAvatar GetAvatar(string id, IUnitAvatar prefab)
        {
            if (!_avatarsPool.TryGetValue(id, out var pool))
            {
                //Logger.Log($"Create new pool for {id}");
                pool = new UnityPool(() => _container.InstantiatePrefabForComponent<IUnitAvatar>(prefab.Object, _sceneModel.Root).Object);

                _avatarsPool.Add(id, pool);
            }

            return pool.GetObject<PoolObject>() as IUnitAvatar;
        }
        
        public IUnitAvatar GetAvatarCompanion(string id, IUnitAvatar prefab)
        {
            if (!_avatarsPool.TryGetValue(id, out var pool))
            {
                Logger.Log($"Create new pool for {id}");
                pool = new UnityPool(() => _container.InstantiatePrefabForComponent<IUnitAvatar>(prefab.Object, _sceneModel.Root).Object);

                _avatarsPool.Add(id, pool);
            }

            return pool.GetObject<PoolObject>() as IUnitAvatar;
        }
    }
}