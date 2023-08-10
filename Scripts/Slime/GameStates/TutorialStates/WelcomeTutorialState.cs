using DG.Tweening;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Enums;
using Slime.UI;
using Slime.UI.Tutorial;

namespace Slime.GameStates.TutorialStates
{
    public class WelcomeTutorialState : State<ETutorialStage>
    {
        private const string MESSAGE = "Welcome to Tank Assault!";

        private readonly ITutorialModel _tutorialModel;
        private readonly UIManager _uiManager;

        public WelcomeTutorialState(ITriggerResponder<ETutorialStage> stateMachine, ITutorialModel tutorialModel,
            UIManager uiManager) : base(stateMachine)
        {
            _tutorialModel = tutorialModel;
            _uiManager = uiManager;
        }

        public override void OnEnter()
        {
            _tutorialModel.DisplayMessage(MESSAGE);

            DOVirtual.DelayedCall(5, () =>
            {
                _uiManager.Close<TutorialView>();
                StateMachine.FireTrigger(ETutorialStage.Attribute);
            });
        }

        public override void OnExit()
        {
        }
    }
}