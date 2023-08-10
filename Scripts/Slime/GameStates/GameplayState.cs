using Slime.AbstractLayer;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Triggers;
using Slime.GameStates.GameplayStates;
using Slime.UI;
using Slime.UI.Common;
using UnityEngine;

namespace Slime.GameStates
{
    public class GameplayState : State<EGameTriggers>, IUpdatable
    {
        private GameplayStateMachine _gameplayStateMachine;

        private readonly IObjectFactory _objectFactory;
        private readonly UIManager _uiManager;

        public GameplayState(IGameStateMachine gameStateMachine,
            IObjectFactory objectFactory,
            UIManager uiManager) : base(gameStateMachine)
        {
            _objectFactory = objectFactory;
            _uiManager = uiManager;
        }

        public override void OnEnter()
        {
            _uiManager.InitializeBaseViews();
            _uiManager.InitializeGameViews();

            // DEBUG: stop gameplay temporarily
            _gameplayStateMachine = new GameplayStateMachine(_objectFactory);
            
            // DEBUG: Disabling sleep mode
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            _uiManager.Close<LoadingView>(false);
        }

        public override void OnExit()
        {
            _gameplayStateMachine?.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            _gameplayStateMachine?.Update(deltaTime);
        }
    }
}