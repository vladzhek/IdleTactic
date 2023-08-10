using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.Data.Abstract;
using Slime.Data.Constants;
using Slime.Data.Enums;
using Slime.Data.Inventory;
using Slime.UI.Common.Equipment;
using UI.Base.MVVM;
using Utils;

namespace Slime.UI.Inventory
{
    public class InventoryViewModel : ViewModel
    {
        private readonly UIManager _uiManager;
        private readonly IInventoryModel _inventoryModel;
        private readonly IInventoryUIModel _inventoryUIModel;
        private readonly IFooterUIModel _footerUIModel;
        private readonly ICharacterUIModel _characterUIModel;
        private readonly ITutorialModel _tutorialModel;
        
        private InventoryViewModel(
            UIManager uiManager, 
            IInventoryModel inventoryModel,
            IInventoryUIModel inventoryUIModel,
            IFooterUIModel footerUIModel,
            ICharacterUIModel characterUIModel,
            ITutorialModel tutorialModel
            )
        {
            _uiManager = uiManager;
            _inventoryModel = inventoryModel;
            _inventoryUIModel = inventoryUIModel;
            _footerUIModel = footerUIModel;
            _characterUIModel = characterUIModel;
            _tutorialModel = tutorialModel;
        }
        
        #region ViewModel implementation

        public override void OnEnable()
        {
            UpdateState();
        }
        
        public override void OnSubscribe()
        {
            base.OnSubscribe();

            _inventoryModel.OnChange += OnInventoryChanged;
            _inventoryUIModel.OnChange += OnInventoryChanged;
        }

        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            _inventoryModel.OnChange -= OnInventoryChanged;
            _inventoryUIModel.OnChange -= OnInventoryChanged;
        }
        
        #endregion

        #region View events

        [UsedImplicitly]
        private void OnUpgradeButtonClicked()
        {
            var selected = GetSelected();
            if (selected == null)
            {
                Logger.Warning("no element is selected");
                return;
            }
            
            _inventoryModel.Upgrade(selected.ID);
        }

        [UsedImplicitly]
        private void OnEquipButtonClicked()
        {
            var selected = GetSelected();
            if (selected == null)
            {
                Logger.Warning("no element is selected");
                return;
            }

            var equippedItem = GetEquipped();
            var shouldEquip = equippedItem?.ID != selected.ID;
            Equip(selected.ID, shouldEquip);
        }

        [UsedImplicitly]
        private void OnUpgradeAllButtonClicked()
        {
            _inventoryModel.UpgradeAll();
        }

        [UsedImplicitly]
        private void OnSummonButtonClicked()
        {
            _uiManager.Close<InventoryView>();
            _footerUIModel.SelectTab(Values.FOOTER_TAB_STORE);
            _characterUIModel.SelectTab(Values.STORE_TAB_SUMMON);
        }

        [UsedImplicitly]
        private void OnCloseButtonClicked()
        {
            _uiManager.Close<InventoryView>();
        }

        #endregion
        
        // public
        
        public EInventoryType Type => _inventoryUIModel.Type;
        public event Action OnInventoryChange;
        public event Action OnSelectionChange;

        public InventoryData GetSelected()
        {
            _selected ??= GetEquipped();
            _selected ??= Get().FirstOrDefault();
            
            return _selected;
        }

        public IEnumerable<ILayoutElementData> Data => from item in Get()
            select ToViewData(item);

        public bool CanSelectedUpgrade => GetSelected()?.CanUpgrade ?? false;
        
        public bool CanAnyUpgrade => Get().Any(item => item.CanUpgrade);
        
        public float TotalEffect => (from item in _inventoryModel.Get()
            where item.IsUnlocked
            select item.PassiveValue * 100).Sum();

        public void Select(string id)
        {
            //Logger.Log($"id: {id}");
            _selected = _inventoryModel.Get(id);
            
            OnSelectionChange?.Invoke();
        }

        public IEnumerable<InventoryData> Get()
        {
            return _inventoryModel.Get(Type); 
        }
        
        public InventoryData GetEquipped()
        {
            return _inventoryModel.GetEquipped(Type);
        }

        public void Equip(string id, bool shouldEquip = true)
        {
            _inventoryModel.Equip(id, shouldEquip);
        }
        
        public bool GetTutorialActive()
        {
            return _tutorialModel.Stage == ETutorialStage.Inventory;
        }
        
        // private
        
        private InventoryData _selected;

        private void OnInventoryChanged()
        {
            Logger.Warning();
            
            UpdateState();
        }
        
        private void UpdateState()
        {
            if (_selected != null)
            {
                // ensure that current selected item is of correct type and has up to date data
                _selected = _selected.Type != Type ? null : _inventoryModel.Get(_selected.ID);
            }
            
            OnInventoryChange?.Invoke();
        }

        private GridLayoutData ToViewData(ISummonable data) => 
            new(data, GetEquipped()?.ID != data.ID 
                      && !(Get().Any(d => d.ActiveValue > data.ActiveValue) 
                           || Get().Any(d => d.PassiveValue > data.PassiveValue)));
    }
}