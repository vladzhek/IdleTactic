using System;
using Slime.AbstractLayer;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Triggers;
using Utils;
using Zenject;

namespace Slime.GameStates
{
    public class GameStateMachine : 
        StateMachineBase<IState, EGameTriggers>, 
        IGameStateMachine,
        IInitializable, 
        IDisposable
    {
        private IObjectFactory _objectFactory;
        private bool _isDisposed;
        
        private GameStateMachine(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        ~GameStateMachine()
        {
            Dispose();
        }

        public override void Initialize()
        {
            base.Initialize();

            var bootstrapState = _objectFactory.CreateObject<BootstrapState>(new object[] { this });
            var loadingState = _objectFactory.CreateObject<LoadingState>(new object[] { this });
            var gameplayState = _objectFactory.CreateObject<GameplayState>(new object[] { this });
            var saveProgressState = _objectFactory.CreateObject<SaveProgressState>(new object[] { this });
            var quitState = _objectFactory.CreateObject<QuitState>(new object[] { this });

            _objectFactory = null;
            
            ConfigureState(bootstrapState)
                .Permit(EGameTriggers.LoadProgress, loadingState);
            ConfigureState(loadingState)
                .Permit(EGameTriggers.Gameplay, gameplayState);
            ConfigureState(gameplayState)
                .Permit(EGameTriggers.SaveProgress, saveProgressState)
                .Permit(EGameTriggers.Quit, quitState);
            ConfigureState(saveProgressState)
                .Permit(EGameTriggers.Gameplay, gameplayState);
            ConfigureState(quitState);
            
            CurrentState = bootstrapState;
            OnEnterState(CurrentState);
        }

        public void OnUpdate(float deltaTime)
        {
            CurrentUpdatableState?.OnUpdate(deltaTime);
        }

        public override void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;

            GoToQuitState();

            base.Dispose();
        }

        private void GoToQuitState()
        {
            FireTrigger(EGameTriggers.Quit);
        }
    }
}