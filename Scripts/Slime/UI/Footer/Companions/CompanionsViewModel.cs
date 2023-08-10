using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Data.Constants;
using Slime.Data.Inventory;
using Slime.UI.Inventory;
using UI.Base.MVVM;

namespace Slime.UI.Footer.Companions
{
    public class CompanionsViewModel : ViewModel
    {
        private readonly ICompanionsModel _companionsModel;
        private readonly UIManager _uiManager;
        private readonly IFooterUIModel _footerUIModel;
        private readonly ICharacterUIModel _characterUIModel;
        private readonly IEquipmentItemUIModel _equipmentItemUIModel;
       

        public event Action OnInventoryChange;
        public event Action<bool> OnActiveChooseSlots;
        public event Action<string> OnPanelItemRemove;

        private bool _isCanEquipPlacement;
        private string _isCanEquipID;
        
        public Dictionary<int, CompanionPanelViewData> CompanionPanelViewData { get; } = new();
        
        public CompanionsViewModel(ICompanionsModel companionsModel,
            UIManager uiManager, 
            IFooterUIModel footerUIModel,
            ICharacterUIModel characterUIModel,
            IEquipmentItemUIModel equipmentItemUIModel)
        {
            _companionsModel = companionsModel;
            _uiManager = uiManager;
            _footerUIModel = footerUIModel;
            _characterUIModel = characterUIModel;
            _equipmentItemUIModel = equipmentItemUIModel;
        }

        public override void OnSubscribe()
        {
            base.OnSubscribe();

            _companionsModel.OnChange += OnInventoryChanged;
            _equipmentItemUIModel.OnEquipRequest += OnEquipRequested;
        }

        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            _companionsModel.OnChange -= OnInventoryChanged;
            _equipmentItemUIModel.OnEquipRequest -= OnEquipRequested;
        }

        private void OnInventoryChanged()
        {
            OnInventoryChange?.Invoke();
        }

        public bool CanAnyUpgrade => Get().Any(item => item.CanUpgrade);
        
        public void Select(string id)
        {
            _equipmentItemUIModel.Open(_companionsModel.Get(id));
        }
        
        public void Equip(string ID, int index, bool shouldEquip = true)
        {
            _companionsModel.Equip(ID, index, shouldEquip);
        }

        public IEnumerable<CompanionData> Get()
        {
            return _companionsModel.Get(); 
        }

        public CompanionData GetEquipped(int index)
        {
            return _companionsModel.GetEquipped(index);
        }

        public void RequestEquip(string id)
        {
            if (FreeSlot() == -1)
            {
                _isCanEquipPlacement = true;
                _isCanEquipID = id;
                OnActiveChooseSlots?.Invoke(_isCanEquipPlacement);
            }
            else
            {
                Equip(id, FreeSlot());
            }
        }
        
        public int FreeSlot()
        {
            var slots = _companionsModel.GetEquipData();
            // NOTE: hardcode
            for (var i = 0; i < _companionsModel.GetEquipmentSlotsCount; i++)
            {
                if (!slots.ContainsKey(i)) return i;
            }
            return -1;
        }

        public int GetCurrentSlot(string id)
        {
            var slots = _companionsModel.GetEquipData();
            return (from slot in slots 
                where slot.Value == id 
                select slot.Key).FirstOrDefault();
        }

        
        public void SelectSlot(int index)
        {
            if (_isCanEquipPlacement)
            {
                Equip(_isCanEquipID, index,false);
                Equip(_isCanEquipID, index);
                DropChooseSlots();
            }
        }

        public void DropChooseSlots()
        {
            _isCanEquipPlacement = false;
            OnActiveChooseSlots?.Invoke(_isCanEquipPlacement);
        }
        
        [UsedImplicitly]
        private void OnUpgradeAllButtonClicked()
        {
            _companionsModel.UpgradeAll();
        }

        [UsedImplicitly]
        private void OnSummonButtonClicked()
        {
            _uiManager.Close<InventoryView>();
            _footerUIModel.SelectTab(Values.FOOTER_TAB_STORE);
            _characterUIModel.SelectTab(Values.STORE_TAB_SUMMON);
        }
        
        private void OnEquipRequested()
        {
            var item = _equipmentItemUIModel.Item;
            if (!item.IsEquipped)
            {
                RequestEquip(item.ID);
            }
            else
            {
                OnPanelItemRemove?.Invoke(item.ID);
                Equip(item.ID, GetCurrentSlot(item.ID), false);
            }
        }
    }
}