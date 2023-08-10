using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Enums;
using Slime.UI;
using Slime.UI.Tutorial;

namespace Slime.GameStates.TutorialStates
{
    public class StageReplayTutorialState : State<ETutorialStage>
    {
        private const string MESSAGE = "Press boss icon to try fighting him again";

        private readonly IStageModel _stageModel;
        private readonly ITutorialModel _tutorialModel;
        private readonly UIManager _uiManager;
        private readonly IStageController _stageController;

        private bool _isWasCycle;

        public StageReplayTutorialState(ITriggerResponder<ETutorialStage> stateMachine,
            IStageModel stageModel,
            ITutorialModel tutorialModel,
            UIManager uiManager,
            IStageController stageController)
            : base(stateMachine)
        {
            _stageModel = stageModel;
            _tutorialModel = tutorialModel;
            _uiManager = uiManager;
            _stageController = stageController;
        }

        public override void OnEnter()
        {
            _stageModel.OnCycleChange += CycleChanged;
            _tutorialModel.SetTutorialType(ETutorialStage.StageReplay);
        }

        public override void OnExit()
        {
            _stageModel.OnCycleChange -= CycleChanged;
        }

        private void CycleChanged(bool isCycle)
        {
            if (isCycle)
            {
                _isWasCycle = true;
                _tutorialModel.DisplayMessage(MESSAGE);
                return;
            }

            if (_isWasCycle)
            {
                _uiManager.Close<TutorialView>();
                StateMachine.FireTrigger(ETutorialStage.Summon);
            }
        }
    }
}