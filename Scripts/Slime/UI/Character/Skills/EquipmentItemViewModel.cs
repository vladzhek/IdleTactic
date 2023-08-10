using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Data.Abstract;
using UI.Base.MVVM;

namespace Slime.UI.Character.Skills
{
    public class EquipmentItemViewModel : ViewModel
    {
        public ISummonable Data => _equipmentItemUIModel.Item;

        public void CloseView()
        {
            _equipmentItemUIModel.Close();
        }

        // private
        
        private readonly IEquipmentItemUIModel _equipmentItemUIModel;
        
        private EquipmentItemViewModel(IEquipmentItemUIModel equipmentItemUIModel)
        {
            _equipmentItemUIModel = equipmentItemUIModel;
        }

        [UsedImplicitly]
        private void OnUpgradeButtonClicked()
        {
            _equipmentItemUIModel.RequestUpgrade();
            CloseView();
        }
        
        [UsedImplicitly]
        private void OnEquipButtonClicked()
        {
            _equipmentItemUIModel.RequestEquip();
            CloseView();
        }
    }
}