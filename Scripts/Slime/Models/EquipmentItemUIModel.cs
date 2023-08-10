using System;
using Slime.AbstractLayer.Models;
using Slime.Data.Abstract;
using Slime.UI;
using Slime.UI.Character.Skills;

namespace Slime.Models
{
    public class EquipmentItemUIModel : IEquipmentItemUIModel
    {
        #region IEquipmentItemUIModel implementation
        
        public event Action OnEquipRequest;
        public event Action OnUpgradeRequest;
        public ISummonable Item { get; private set; }
        
        public void Open(ISummonable item)
        {
            Item = item;
            _uiManager.Open<EquipmentItemView>();
        }

        public void Close()
        {
            _uiManager.Close<EquipmentItemView>();
        }
        
        public void RequestEquip()
        {
            OnEquipRequest?.Invoke();
        }

        public void RequestUpgrade()
        {
            OnUpgradeRequest?.Invoke();
        }
        
        #endregion
        
        // private

        private readonly UIManager _uiManager;

        private EquipmentItemUIModel(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
    }
}