using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.Data.Abstract;
using Slime.Data.Constants;
using Slime.Data.Skills;
using Slime.UI.Common.Equipment;
using Slime.UI.Inventory;
using UI.Base.MVVM;
using Utils;
using Utils.Extensions;

namespace Slime.UI.Character.Skills
{
    public class SkillsViewModel : ViewModel
    {
        private readonly UIManager _uiManager;
        private readonly ISkillsModel _skillsModel;
        private readonly IFooterUIModel _footerUIModel;
        private readonly ISkillsManager _skillsManager;
        private readonly IEquipmentItemUIModel _equipmentItemUIModel;
        private readonly ICharacterUIModel _characterUIModel;
        private bool _isCanEquip;

        private string _itemSelectedKey;

        public Dictionary<string,ILayoutElementData> SkillsGridViewData { get; } = new();
        public Dictionary<int, SkillPanelViewData> SkillPanelViewData { get; } = new();

        public SkillsViewModel(
            UIManager uiManager,
            ISkillsModel skillsModel,
            IFooterUIModel footerUIModel,
            ISkillsManager skillsManager,
            IEquipmentItemUIModel equipmentItemUIModel,
            ICharacterUIModel characterUIModel)
        {
            _uiManager = uiManager;
            _skillsModel = skillsModel;
            _footerUIModel = footerUIModel;
            _skillsManager = skillsManager;
            _equipmentItemUIModel = equipmentItemUIModel;
            _characterUIModel = characterUIModel;
        }
        
        public bool CanAnyUpgrade => _skillsModel.Get().Any(item => item.CanUpgrade);

        public event Action OnSkillsUpdate;
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            
            InitializeSkillsGridData();
            InitializeSkillsPanelData();
        }

        public override void OnSubscribe()
        {
            base.OnSubscribe();

            _skillsModel.OnChange += UpdateState;
            _equipmentItemUIModel.OnEquipRequest += ItemEquipRequested;
            _equipmentItemUIModel.OnUpgradeRequest += ItemUpgradeRequested;
            
            UpdateState();
        }

        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            _skillsModel.OnChange -= UpdateState;
            _equipmentItemUIModel.OnEquipRequest -= ItemEquipRequested;
            _equipmentItemUIModel.OnUpgradeRequest -= ItemUpgradeRequested;
        }

        private void UpdateState()
        {
            if (!_skillsModel.IsLoaded)
            {
                return;
            }
            
            InitializeSkillsGridData();
            InitializeSkillsPanelData();
            OnSkillsUpdate?.Invoke();
        }

        private void InitializeSkillsGridData()
        {
            SkillsGridViewData.Clear();
            foreach (var item in _skillsModel.Get())
            {
                SkillsGridViewData[item.ID] = new GridLayoutData(item);
            }
        }

        private void InitializeSkillsPanelData()
        {
            for (var index = 0; index < _skillsManager.MaxSkillsCount; index++)
            {
                var data = _skillsModel.GetEquipped(index);
                var viewData = data != null 
                    ? new SkillPanelViewData 
                    {
                        ID = data.ID,
                        Sprite = data.Sprite,
                        IsOpened = true, // ?
                        IsEquipped = true, // NOTE: why skill panel need isEquipped if it contains only equipped? 
                        Cooldown = data.Cooldown, 
                    } 
                    : new SkillPanelViewData
                    {
                        IsOpened = true,
                        ID = $"{index}",
                    };

                SkillPanelViewData[index] = viewData;
            }
        }

        public void OnEquipSkillGridElement(ILayoutElementData element)
        {
            _itemSelectedKey = element.ID;
            ShowAvailableSkillSlots(true);
            _isCanEquip = true;
        }

        public void OnSelectedSkillPanelElement(SkillPanelViewData element)
        {
            if (_isCanEquip)
            {
                _skillsModel.Equip(_itemSelectedKey, GetSelectedIndex(element.ID));
                ShowAvailableSkillSlots(false);
                _isCanEquip = false;
            }
        }

        public void SelectedSkillGridElement(ILayoutElementData element)
        {
            _equipmentItemUIModel.Open(_skillsModel.Get(element.ID));
            _itemSelectedKey = element.ID;
            _isCanEquip = true;
        }
        
        public void OnUnequipSkillGridElement(ILayoutElementData element)
        {
            for (var i = 0; i < SkillPanelViewData.Count; i++)
            {
                if (SkillPanelViewData[i].ID != element.ID) continue;
                _skillsModel.Equip(element.ID, i, false);
                UpdateState();
                return;
            }
        }

        private int GetSelectedIndex(string id)
        {
            for (int i = 0; i < SkillPanelViewData.Count; i++)
            {
                if (SkillPanelViewData[i].ID == id)
                {
                    return i;
                }
            }

            return -1;
        }

        [UsedImplicitly]
        public void OnSummonButtonClicked()
        {
            _uiManager.Close<InventoryView>();
            _footerUIModel.SelectTab(Values.FOOTER_TAB_STORE);
            _characterUIModel.SelectTab(Values.STORE_TAB_SUMMON);
        }

        [UsedImplicitly]
        public void OnUpgradeButtonClicked()
        {
            _skillsModel.UpgradeAll();
        }

        public float GetTotalEffect()
        {
            return _skillsModel.GetTotalPassiveEffect() * 100;
        }
        
        public void ShowAvailableSkillSlots(bool isActive)
        {
            foreach (var skillPanelViewData in 
                     from item in SkillPanelViewData 
                     where item.Value.IsOpened 
                     select item)
            {
                skillPanelViewData.Value.IsCanEquipped = isActive;
            }
            
            OnSkillsUpdate?.Invoke();
        }

        private void ItemEquipRequested()
        {
            // TODO: check that item in question is skill
            ShowAvailableSkillSlots(true);
        }
        
        private void ItemUpgradeRequested()
        {
            _skillsModel.Upgrade(_equipmentItemUIModel.Item.ID);
        }
    }
}