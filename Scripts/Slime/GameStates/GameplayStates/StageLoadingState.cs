using DG.Tweening;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Triggers;

namespace Slime.GameStates.GameplayStates
{
    [UsedImplicitly]
    public class StageLoadingState : State<EGameplayTriggers>
    {
        private readonly IUnitsModel _unitsModel;
        private readonly IStageController _stageController;
        private readonly IStageModel _stageModel;

        protected StageLoadingState(GameplayStateMachine stateMachine, IStageController stageController,
            IUnitsModel unitsModel, IStageModel stageModel) : base(stateMachine)
        {
            _stageController = stageController;
            _unitsModel = unitsModel;
            _stageModel = stageModel;
        }

        public override void OnEnter()
        {
            // NOTE: Black screen is representation, not logic - what is needed here?
            var isBlackScreenNeeded = _stageController.CurrentStage == null ||
                                      _stageController.CurrentStage.StageConfig.StageType != _stageModel.Type 
                                      || (!_stageModel.IsBattleCycled && _stageController.CurrentStage.IsLastWave)
                                      || _stageController.CurrentStage.IsWaveFailed;
            
            if (isBlackScreenNeeded)
            {
                _stageModel.SetScreenHidden(true);

                DOVirtual.DelayedCall(1, LoadLevelFromScratch);
            }
            else
            {
                DOVirtual.DelayedCall(1, LoadLevel);
            }
        }

        public override void OnExit()
        {
        }

        private void LoadLevelFromScratch()
        {
            _unitsModel.ClearEnemyUnits();
            _stageController.LoadStageFromScratch();

            StateMachine.FireTrigger(EGameplayTriggers.Fight);
        }

        private void LoadLevel()
        {
            _unitsModel.ClearEnemyUnits();
            _stageController.LoadNextLevel();

            StateMachine.FireTrigger(EGameplayTriggers.Fight);
        }
    }
}