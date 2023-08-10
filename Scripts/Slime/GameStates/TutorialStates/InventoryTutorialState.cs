using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Enums;
using Slime.UI;
using Slime.UI.Tutorial;

namespace Slime.GameStates.TutorialStates
{
    public class InventoryTutorialState : State<ETutorialStage>
    {
        private const string MESSAGE = "Equip items to become more powerful!";

        private readonly ITutorialModel _tutorialModel;
        private readonly UIManager _uiManager;
        private readonly IInventoryModel _inventoryModel;

        public InventoryTutorialState(ITriggerResponder<ETutorialStage> stateMachine, ITutorialModel tutorialModel,
            UIManager uiManager, IInventoryModel inventoryModel) : base(stateMachine)
        {
            _tutorialModel = tutorialModel;
            _uiManager = uiManager;
            _inventoryModel = inventoryModel;
        }

        public override void OnEnter()
        {
            _inventoryModel.OnEquip += Equipped;

            _tutorialModel.SetTutorialType(ETutorialStage.Inventory);
            _tutorialModel.DisplayMessage(MESSAGE);
        }

        public override void OnExit()
        {
        }

        private void Equipped(string arg1, bool arg2)
        {
            _inventoryModel.OnEquip -= Equipped;
            
            _uiManager.Close<TutorialView>();

            StateMachine.FireTrigger(ETutorialStage.Final);
        }
    }
}