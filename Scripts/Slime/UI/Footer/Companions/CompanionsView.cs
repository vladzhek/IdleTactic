using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ModestTree;
using Slime.Data.Abstract;
using Slime.UI.Common;
using Slime.UI.Common.Equipment;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

namespace Slime.UI.Footer.Companions
{
    public class CompanionsView : View<CompanionsViewModel>
    {
        [SerializeField] private EquipmentLayout _equipmentLayout;
        [SerializeField] private TotalEffectWidget _totalEffectWidget;
        [SerializeField] private List<PlacementWidget> _platforms;
        [UsedImplicitly, SerializeField] private GenericButton _upgradeAllButton;
        [UsedImplicitly, SerializeField] private GenericButton _summonButton;
        [SerializeField] private GameObject _buttonDrop;
        public override UILayer Layer => UILayer.Middleground;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            UpdateState();
            OnCompanionsChanged();
            
            _buttonDrop.SetActive(false); 
            _buttonDrop.GetComponent<Button>().onClick.AddListener(DropChooseSlots);
        }

        private void DropChooseSlots()
        {
            ViewModel.DropChooseSlots();
        }

        private void UpdateState()
        {
            var items = ViewModel.Get().ToArray();
            _equipmentLayout.SetData(from item in items select new GridLayoutData(item));
            _totalEffectWidget.SetValue((from item in items where item.IsUnlocked select item.PassiveValue).Sum());

            _upgradeAllButton.Interactable = ViewModel.CanAnyUpgrade;
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            ViewModel.OnInventoryChange += OnInventoryChanged;
            ViewModel.OnPanelItemRemove += OnInventoryRemove;
            ViewModel.OnActiveChooseSlots += OnActivePlatforms;
            _equipmentLayout.OnSelect += OnSelectButtonClicked;
            _equipmentLayout.OnAddButtonClick += OnAddButtonClicked;
            _equipmentLayout.OnRemoveButtonClick += OnRemoveButtonClicked;
        }
        
        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            ViewModel.OnInventoryChange -= OnInventoryChanged;
            ViewModel.OnPanelItemRemove -= OnInventoryRemove;
            ViewModel.OnActiveChooseSlots -= OnActivePlatforms;
            _equipmentLayout.OnSelect -= OnSelectButtonClicked;
            _equipmentLayout.OnAddButtonClick -= OnAddButtonClicked;
            _equipmentLayout.OnRemoveButtonClick -= OnRemoveButtonClicked;
        }

        private void OnActivePlatforms(bool isActive)
        {
            foreach (var platform in _platforms)
            {
                platform.ActiveSlot(isActive);
            }
            
            _buttonDrop.SetActive(isActive);
        }

        private void OnInventoryChanged()
        {
            UpdateState();
            OnCompanionsChanged();
        }

        private void OnCompanionsChanged()
        {
            for (var i = 0; i < _platforms.Count; i++)
            {
                _platforms[i].SetActive(false);
                
                var data = ViewModel.GetEquipped(i);
                if(data == null)
                    continue;
                _platforms[i].SetCompanion(data.Sprite,data.Rarity, i);
                _platforms[i].OnPlacementClick += OnPlacementClicked;
            }
        }

        private void OnPlacementClicked(int index)
        {
            ViewModel.SelectSlot(index);
        }

        private void OnSelectButtonClicked(ILayoutElementData data)
        {
            ViewModel.Select(data.ID);
        }
        
        private void OnAddButtonClicked(ILayoutElementData data)
        {
            ViewModel.RequestEquip(data.ID);
        }
        
        private void OnRemoveButtonClicked(ILayoutElementData data)
        {
            OnInventoryRemove(data.ID);
            ViewModel.Equip(data.ID, ViewModel.GetCurrentSlot(data.ID),false);
        }

        private void OnInventoryRemove(string id)
        {
            _platforms[ViewModel.GetCurrentSlot(id)].SetActive(false);
        }
        
    }
}