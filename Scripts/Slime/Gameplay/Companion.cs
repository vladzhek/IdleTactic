using System.Collections.Generic;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Models;
using Slime.Factories;
using Slime.Gameplay.Battle.Units;
using UnityEngine;
using UnityEngine.Rendering;

namespace Slime.Gameplay
{
    public class Companion : ICompanion
    {
        private readonly ICompanionsModel _companionsModel;
        private readonly UnitFactory _unitFactory;
        private readonly UnitBase _unitBase;
        private readonly ISceneModel _sceneModel;
        public Dictionary<int,IUnit> Units { get; set; }

        public Companion(ICompanionsModel companionsModel,
            UnitFactory unitFactory,
            ISceneModel sceneModel)
        {
            _companionsModel = companionsModel;
            _unitFactory = unitFactory;
            _sceneModel = sceneModel;

            Subscribe();
            Units ??= new Dictionary<int, IUnit>();
        }

        public void Subscribe()
        {
            _companionsModel.OnEquip += AddCompanion;
            _companionsModel.OnRemove += DestroyCompanion;
        }
        
        public void UnSubscribe()
        {
            _companionsModel.OnEquip -= AddCompanion;
            _companionsModel.OnRemove -= DestroyCompanion;
        }

        public void AddUnit(int index, IUnit unit)
        {
            if(Units.ContainsKey(index))
                return;

            Units ??= new Dictionary<int, IUnit>();
            Units.Add(index,unit);
        }

        public void OnUpdate(float delta)
        {
            if(Units == null)
                return;
            
            foreach (var unit in Units)
            {
                unit.Value.OnUpdate(delta);
            }
        }

        public void CreateCompanions()
        {
            foreach (var data in _companionsModel.GetEquipData())
            {
                if(Units.ContainsKey(data.Key))
                    continue;
                
                var unit = _unitFactory.CreateCompanionUnit(data.Value, data.Key);
                AddUnit(data.Key, unit);
                Units[data.Key].InjectAvatar(_unitFactory.CreateCompanionAvatar(data.Value));
                Units[data.Key].SetPosition(Vector3.zero, Quaternion.identity);
            }
        }

        public void DestroyCompanion(int index)
        {
            Units[index].Avatar.TriggerDestroy();
            Units[index].SetTarget(null);
            Units[index].DestroyUnit();
            Units.Remove(index);
        }

        public void AddCompanion(int index)
        {
            var ID = _companionsModel.GetEquipData()[index];
            var unit = _unitFactory.CreateCompanionUnit(ID, index);
            AddUnit(index, unit);
            Units[index].InjectAvatar(_unitFactory.CreateCompanionAvatar(ID));
            Units[index].SetPosition(_sceneModel.CompanionsPositions[index].position, _sceneModel.CompanionsPositions[index].rotation);
        }
    }
}