using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Enums;
using Slime.UI;
using Slime.UI.Tutorial;

namespace Slime.GameStates.TutorialStates
{
    public class SummonTutorialState : State<ETutorialStage>
    {
        private const string MESSAGE = "You can summon equipment in store, try to get more weapons";

        private readonly ISummonModel _summonModel;
        private readonly ITutorialModel _tutorialModel;
        private readonly UIManager _uiManager;

        public SummonTutorialState(ITriggerResponder<ETutorialStage> stateMachine, ISummonModel summonModel,
            ITutorialModel tutorialModel, UIManager uiManager) : base(stateMachine)
        {
            _summonModel = summonModel;
            _tutorialModel = tutorialModel;
            _uiManager = uiManager;
        }

        public override void OnEnter()
        {
            _summonModel.OnSummon += Summon;
            _tutorialModel.SetTutorialType(ETutorialStage.Summon);

            _tutorialModel.DisplayMessage(MESSAGE);
        }

        public override void OnExit()
        {
        }

        private void Summon(ESummonType type)
        {
            _summonModel.OnSummon -= Summon;

            _uiManager.Close<TutorialView>();
            
            StateMachine.FireTrigger(ETutorialStage.Inventory);
        }
    }
}