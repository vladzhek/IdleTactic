using System;
using Slime.AbstractLayer;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Triggers;

namespace Slime.GameStates.GameplayStates
{
    public sealed class GameplayStateMachine : StateMachineBase<IState, EGameplayTriggers>
    {
        private PlayerCreationState PlayerCreationState { get; }
        private FightState FightState { get; }
        private StageLoadingState StageLoadingState { get; }

        public GameplayStateMachine(IObjectFactory objectFactory)
        {
            Initialize();

            PlayerCreationState = objectFactory.CreateObject<PlayerCreationState>(new object[] { this });
            FightState = objectFactory.CreateObject<FightState>(new object[] { this });
            StageLoadingState = objectFactory.CreateObject<StageLoadingState>(new object[] { this });

            ConfigureState(PlayerCreationState)
                .Permit(EGameplayTriggers.LoadStage, StageLoadingState);
            ConfigureState(StageLoadingState)
                .Permit(EGameplayTriggers.PlayerCreation, PlayerCreationState)
                .Permit(EGameplayTriggers.Fight, FightState);
            ConfigureState(FightState)
                .Permit(EGameplayTriggers.LoadStage, StageLoadingState)
                .Permit(EGameplayTriggers.PlayerCreation, PlayerCreationState);

            CurrentState = PlayerCreationState;
            OnEnterState(CurrentState);
        }

        public void Update(float deltaTime)
        {
            CurrentUpdatableState?.OnUpdate(deltaTime);
        }
    }
}