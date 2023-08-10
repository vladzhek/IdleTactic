using JetBrains.Annotations;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.AbstractLayer.StateMachine;
using Slime.Configs;
using Slime.Data.IDs;
using Slime.Data.Triggers;
using Slime.Factories;
using Slime.Services;
using UnityEngine;

namespace Slime.GameStates
{
    [UsedImplicitly]
    public class LoadingState : State<EGameTriggers>
    {
        private readonly IGameProgressModel _progressModel;
        private readonly IStageController _stageController;
        private readonly GameConfig _gameConfig;
        private readonly IPlayer _player;
        private readonly ICompanion _companion;
        private readonly UnitFactory _unitFactory;
        private readonly ICompanionsModel _companionsModel;

        public LoadingState(IGameStateMachine gameStateMachine,
            IGameProgressModel progressModel,
            IStageController stageController,
            GameConfig gameConfig,
            IPlayer player,
            ICompanion companion,
            ICompanionsModel companionsModel,
            UnitFactory unitFactory) : base(gameStateMachine)
        {
            _progressModel = progressModel;

            _stageController = stageController;
            _gameConfig = gameConfig;
            _player = player;
            _companion = companion;
            _unitFactory = unitFactory;
            _companionsModel = companionsModel;
        }

        public override void OnEnter()
        {
            Load();
        }

        public override void OnExit()
        {
        }

        private async void Load()
        {
            // TODO: loading queue with all appropriate initializations (ProgressService/ProgressModel, Skills, etc.)
            await _progressModel.Load();

            // TODO: should not be done here, but in CharacterModel (IPlayer?)
            CreatePlayerUnit();

            // TODO: each model should get its own configs
            _stageController.SetConfigs(_gameConfig.Stages.Levels);

            StateMachine.FireTrigger(EGameTriggers.Gameplay);
        }

        private void CreatePlayerUnit()
        {
            // TODO: replace with ICharacterModel.SelectedCharacter
            var unit = _unitFactory.CreateCharacterUnit(CharacterIDs.Default);
            _player.SetUnit(unit);
            
            /*foreach (var attributeId in AttributeIDs.Upgradable)
            {
                var value = _upgradesModel.GetCurrentValue(attributeId);
                Logger.Log($"attribute: {attributeId}; value: {value}");
                _player.Unit.GetAttribute(attributeId).SetValue(value);
            }*/

            _player.Unit.ResetParameters();

            //CreateCompanionUnits();
            _companion.CreateCompanions();
        }

        private void CreateCompanionUnits()
        {
            foreach (var data in _companionsModel.GetEquipData())
            {
                if(_companion.Units.ContainsKey(data.Key))
                    continue;
                
                var unit = _unitFactory.CreateCompanionUnit(data.Value, data.Key);
                _companion.AddUnit(data.Key, unit);
                _companion.Units[data.Key].InjectAvatar(_unitFactory.CreateCompanionAvatar(data.Value));
                _companion.Units[data.Key].SetPosition(Vector3.zero, Quaternion.identity);
            }
        }
    }
}