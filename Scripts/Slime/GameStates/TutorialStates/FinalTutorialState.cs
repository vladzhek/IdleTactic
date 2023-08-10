using DG.Tweening;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Enums;
using Slime.UI;
using Slime.UI.Tutorial;

namespace Slime.GameStates.TutorialStates
{
    public class FinalTutorialState : State<ETutorialStage>
    {
        private const string MESSAGE = "Summon equipment, companions, skills to defeat tough enemies!";

        private readonly ITutorialModel _tutorialModel;
        private readonly UIManager _uiManager;

        public FinalTutorialState(ITriggerResponder<ETutorialStage> stateMachine, ITutorialModel tutorialModel,
            UIManager uiManager) : base(stateMachine)
        {
            _tutorialModel = tutorialModel;
            _uiManager = uiManager;
        }

        public override void OnEnter()
        {
            _tutorialModel.DisplayMessage(MESSAGE);
            _tutorialModel.SetTutorialType(ETutorialStage.Final);


            DOVirtual.DelayedCall(3, () =>
            {
                _tutorialModel.SetTutorialType(ETutorialStage.Complete);
                _uiManager.Close<TutorialView>();
            });
        }

        public override void OnExit()
        {
        }
    }
}