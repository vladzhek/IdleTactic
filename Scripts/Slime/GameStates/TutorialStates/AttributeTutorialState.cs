using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Attributes;
using Slime.Data.Enums;
using Slime.UI;
using Slime.UI.Tutorial;

namespace Slime.GameStates.TutorialStates
{
    public class AttributeTutorialState : State<ETutorialStage>
    {
        private const string MESSAGE = "Upgrade your ATK attribute to become stronger!";

        private readonly ITutorialModel _tutorialModel;
        private readonly UIManager _uiManager;
        private readonly IAttributesModel _attributesModel;

        public AttributeTutorialState(
            ITriggerResponder<ETutorialStage> stateMachine, 
            ITutorialModel tutorialModel,
            UIManager uiManager, 
            IAttributesModel attributesModel) : base(stateMachine)
        {
            _tutorialModel = tutorialModel;
            _uiManager = uiManager;
            _attributesModel = attributesModel;
        }

        public override void OnEnter()
        {
            _tutorialModel.DisplayMessage(MESSAGE);
            _tutorialModel.SetTutorialType(ETutorialStage.Attribute);

            _attributesModel.OnUpgrade += OnUpgraded;
        }

        public override void OnExit()
        {
            _attributesModel.OnUpgrade -= OnUpgraded;
        }

        private void OnUpgraded(AttributeData _)
        {
            _uiManager.Close<TutorialView>();

            //StateMachine.FireTrigger(ETutorialStage.StageReplay);
            StateMachine.FireTrigger(ETutorialStage.Summon);
        }
    }
}