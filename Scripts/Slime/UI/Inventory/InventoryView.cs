using System;
using System.Linq;
using JetBrains.Annotations;
using Slime.Data.Abstract;
using Slime.Data.Constants;
using Slime.Data.Enums;
using Slime.Data.Inventory;
using Slime.UI.Common;
using Slime.UI.Common.Equipment;
using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;
using Logger = Utils.Logger;

namespace Slime.UI.Inventory
{
    public class InventoryView : View<InventoryViewModel>
    {
        public override UILayer Layer => UILayer.Overlay;

        [SerializeField] private TextMeshProUGUI _sectionTitle;

        [Header("Header")] [SerializeField] private GridLayoutElement _selectedElement;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _activeAttack;
        [SerializeField] private TextMeshProUGUI _passiveAttack;
        [SerializeField] private TextMeshProUGUI _activeAttackDelta;
        [SerializeField] private TextMeshProUGUI _passiveAttackDelta;
        [SerializeField] private ToggleWidget _activeDeltaWidget;
        [SerializeField] private ToggleWidget _passiveDeltaWidget;

        [UsedImplicitly, SerializeField] private GenericButton _upgradeButton;
        [UsedImplicitly, SerializeField] private GenericButton _equipButton;

        [Header("Body")] [SerializeField] private TotalEffectWidget _totalEffectWidget;
        [SerializeField] private EquipmentLayout _equipmentLayout;

        [Header("Footer")] [UsedImplicitly, SerializeField]
        private GenericButton _upgradeAllButton;

        [UsedImplicitly, SerializeField] private GenericButton _summonButton;
        [UsedImplicitly, SerializeField] private Button _closeButton;

        #region View implementation

        protected override void Start()
        {
            base.Start();

            InitState();
            UpdateState();
            
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            
            ViewModel.OnInventoryChange += OnInventoryChanged;
            ViewModel.OnSelectionChange += OnSelectionChanged;
            _equipmentLayout.OnSelect += OnSelectButtonClicked;
            _equipmentLayout.OnAddButtonClick += OnAddButtonClicked;
            _equipmentLayout.OnRemoveButtonClick += OnRemoveButtonClicked;

        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();

            ViewModel.OnInventoryChange -= OnInventoryChanged;
            ViewModel.OnSelectionChange -= OnSelectionChanged;
            _equipmentLayout.OnSelect -= OnSelectButtonClicked;
            _equipmentLayout.OnAddButtonClick -= OnAddButtonClicked;
            _equipmentLayout.OnRemoveButtonClick -= OnRemoveButtonClicked;
        }

        #endregion

        private void OnInventoryChanged()
        {
            // NOTE: proper update, this one refreshes whole view
            UpdateState();
        }

        private void OnSelectionChanged()
        {
            // NOTE: proper update, this one refreshes whole view
            UpdateState();
        }

        private void OnSelectButtonClicked(ILayoutElementData data)
        {
            ViewModel.Select(data.ID);
        }

        private void OnAddButtonClicked(ILayoutElementData data)
        {
            ViewModel.Equip(data.ID);
        }

        private void OnRemoveButtonClicked(ILayoutElementData data)
        {
            ViewModel.Equip(data.ID, false);
        }

        private void InitState()
        {
            _sectionTitle.text = ViewModel.Type.GetTitle();

            _activeAttackDelta.gameObject.SetActive(false);
            _activeDeltaWidget.SetInactive();
            _passiveAttackDelta.gameObject.SetActive(false);
            _passiveDeltaWidget.SetInactive();
        }

        private void UpdateState()
        {
            var isArmor = ViewModel.Type == EInventoryType.Armor;
            var (template, activeTemplate, passiveTemplate) = isArmor
                ? (Strings.DEF, Strings.ACTIVE_DEF, Strings.PASSIVE_DEF)
                : (Strings.ATK, Strings.ACTIVE_ATK, Strings.PASSIVE_ATK);

            // all items
            _upgradeAllButton.Interactable = ViewModel.CanAnyUpgrade;
            _equipmentLayout.SetData(ViewModel.Data);
            _totalEffectWidget.SetValue(ViewModel.TotalEffect, template);

            SetTutorialActive();

            // selected item
            _upgradeButton.Interactable = ViewModel.CanSelectedUpgrade;
            var selectedItem = ViewModel.GetSelected();
            var isSelected = selectedItem != null;
            if (!isSelected)
            {
                Logger.Warning($"selected item is null");
                // NOTE: hide selection block?
                return;
            }
            
            _selectedElement.SetData(new GridLayoutData(selectedItem));
            _itemName.text = selectedItem.Title;
            _activeAttack.text = activeTemplate.Resolve((selectedItem.ActiveValue * 100).ToMetricPrefixString());
            _passiveAttack.text = passiveTemplate.Resolve((selectedItem.PassiveValue * 100).ToMetricPrefixString());

            // equipped item
            var equippedItem = ViewModel.GetEquipped();
            _equipButton.Title = equippedItem?.ID == selectedItem.ID ? "unequip" : "equip";
            if (equippedItem == null)
            {
                //Logger.Warning($"equipped item is null");
                return;
            }

            ProcessDelta(_activeAttackDelta, _activeDeltaWidget, equippedItem.ActiveValue, selectedItem.ActiveValue);
            ProcessDelta(_passiveAttackDelta, _passiveDeltaWidget, equippedItem.PassiveValue,
                selectedItem.PassiveValue);
        }

        private void SetTutorialActive()
        {
            // TODO: this throws null
            _equipmentLayout.Elements.Find(x => x.Data.IsUnlocked)
                .SetTutorialFingerActive(ViewModel.GetTutorialActive());
        }

        private static void ProcessDelta(TMP_Text tmp, ToggleWidget toggleWidget, float equippedValue,
            float selectedValue)
        {
            var isEqual = Math.Abs(equippedValue - selectedValue) < 0.001f;
            tmp.gameObject.SetActive(!isEqual);

            // TODO: move colors to Constants.Colors
            var isBetter = equippedValue < selectedValue;
            var color = isBetter ? Color.green : Color.red;
            tmp.color = color;
            var sign = equippedValue > selectedValue ? "-" : "+";
            tmp.text = $"{sign} {Mathf.Abs(equippedValue - selectedValue):F2}%";

            if (isEqual) toggleWidget.SetInactive();
            else toggleWidget.SetActive(isBetter);
        }
    }
}