using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Configs;
using Slime.Data.Triggers;
using Slime.Factories;
using Slime.Services;
using Utils;

namespace Slime.GameStates.GameplayStates
{
    public class PlayerCreationState : State<EGameplayTriggers>
    {
        private readonly UnitFactory _unitFactory;
        private readonly IPlayer _player;
        private readonly ICompanion _companion;
        private readonly ISceneModel _sceneModel;
        private readonly ICompanionsModel _companionsModel;
        private readonly GameConfig _gameConfig;

        private bool _isFirstCreation = true;

        protected PlayerCreationState(GameplayStateMachine stateMachine, 
            UnitFactory unitFactory, 
            IPlayer player,
            ISceneModel sceneModel, 
            ICompanionsModel companionsModel,
            ICompanion companion,
            GameConfig gameConfig) : base(stateMachine)
        {
            _sceneModel = sceneModel;
            _player = player;
            _companion = companion;
            _unitFactory = unitFactory;
            _companionsModel = companionsModel;
            _gameConfig = gameConfig;
        }

        public override void OnEnter()
        {
            if (_isFirstCreation)
            {
                foreach (var (index, companion) in _companionsModel.GetEquipData())
                {
                    CreateCompanionsAvatar(index, companion);
                }
                
                _isFirstCreation = false;
            }
            else
            {
                ResetPlayer();
                //ResetCompanions();
            }

            StateMachine.FireTrigger(EGameplayTriggers.LoadStage);
        }

        public override void OnExit()
        {
        }

        private void ResetPlayer()
        {
            _player.Unit.Reset();
            _player.Unit.SetPosition(_sceneModel.PlayerPosition.position, _sceneModel.PlayerPosition.rotation);
        }

        private void CreateCompanionsAvatar(int index, string ID)
        {
            var companionPrefab = _gameConfig.Companions.TryGetValue(ID, out var prefab) ? prefab : null;
            if (companionPrefab != null)
            {
                _companion.Units[index].InjectAvatar(CreateCompanionAvatar(ID));
                _companion.Units[index].SetPosition(_sceneModel.CompanionsPositions[index].position, _sceneModel.CompanionsPositions[index].rotation);
            }
            else
            {
                Logger.Error($"No avatar for {ID} in player's units");
            }
        }

        private IUnitAvatar CreateCompanionAvatar(string id)
        {
            if (!_gameConfig.Companions.TryGetValue(id, out var prefab))
            {
                Logger.Error($"No avatar for {id} in player's units");
                return null;
            }

            return _unitFactory.GetAvatarCompanion(id, prefab);
        }
    }
}