using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Enums;
using Slime.GameStates.TutorialStates;

namespace Slime.GameStates
{
    public sealed class TutorialStateMachine : StateMachineBase<IState, ETutorialStage>
    {
        public TutorialStateMachine(IObjectFactory objectFactory, ITutorialModel tutorialModel)
        {
            Initialize();

            var welcomeTutorialState = objectFactory.CreateObject<WelcomeTutorialState>(new object[] { this });
            var attributeTutorialState = objectFactory.CreateObject<AttributeTutorialState>(new object[] { this });
            var stageReplayTutorialState = objectFactory.CreateObject<StageReplayTutorialState>(new object[] { this });
            var summonTutorialState = objectFactory.CreateObject<SummonTutorialState>(new object[] { this });
            var inventoryTutorialState = objectFactory.CreateObject<InventoryTutorialState>(new object[] { this });
            var finalTutorialState = objectFactory.CreateObject<FinalTutorialState>(new object[] { this });

            ConfigureState(welcomeTutorialState)
                .Permit(ETutorialStage.Attribute, attributeTutorialState);
            ConfigureState(attributeTutorialState)
                .Permit(ETutorialStage.StageReplay, stageReplayTutorialState)
                .Permit(ETutorialStage.Summon, summonTutorialState);
            ConfigureState(stageReplayTutorialState)
                .Permit(ETutorialStage.Summon, summonTutorialState);
            ConfigureState(summonTutorialState)
                .Permit(ETutorialStage.Inventory, inventoryTutorialState);
            ConfigureState(inventoryTutorialState)
                .Permit(ETutorialStage.Final, finalTutorialState);
            ConfigureState(finalTutorialState);

            CurrentState = tutorialModel.Stage switch
            {
                ETutorialStage.Welcome => welcomeTutorialState,
                ETutorialStage.Attribute => attributeTutorialState,
                ETutorialStage.StageReplay => stageReplayTutorialState,
                ETutorialStage.Summon => summonTutorialState,
                ETutorialStage.Inventory => inventoryTutorialState,
                ETutorialStage.Final => finalTutorialState,
                _ => CurrentState
            };

            if (CurrentState == null)
            {
                return;
            }

            OnEnterState(CurrentState);
        }
    }
}