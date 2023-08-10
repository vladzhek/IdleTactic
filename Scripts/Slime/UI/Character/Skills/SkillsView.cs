using System;
using JetBrains.Annotations;
using Slime.Data.Abstract;
using Slime.Data.Constants;
using Slime.Data.Skills;
using Slime.UI.Battle;
using Slime.UI.Common;
using Slime.UI.Common.Equipment;
using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;
using Logger = Utils.Logger;

namespace Slime.UI.Character.Skills
{
    public class SkillsView : View<SkillsViewModel>
    {
        // TODO: generic grid widget
        [SerializeField] private EquipmentLayout _skillGridWidget;
        [SerializeField] private SkillsPanel _skillsPanel;
        [SerializeField] private GenericButton _upgradeButton;
        [SerializeField, UsedImplicitly] private GenericButton _summonButton;
        [SerializeField] private TotalEffectWidget _totalEffectWidget;

        public override UILayer Layer => UILayer.CharacterTabbar;

        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateData();
            
            _upgradeButton.interactable = ViewModel.CanAnyUpgrade;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            
            ViewModel.OnSkillsUpdate += OnSkillsChanged;

            _skillGridWidget.OnSelect += OnSelectedSkillGridElement;
            _skillGridWidget.OnAddButtonClick += OnEquipButtonClick;
            _skillsPanel.OnSelect += OnSelectedSkillPanelElement;
            _skillGridWidget.OnRemoveButtonClick += UnequipButtonClick;
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            ViewModel.OnSkillsUpdate -= OnSkillsChanged;
            
            _skillGridWidget.OnSelect -= OnSelectedSkillGridElement;
            _skillsPanel.OnSelect -= OnSelectedSkillPanelElement;
            _skillGridWidget.OnAddButtonClick -= OnEquipButtonClick;
            _skillGridWidget.OnRemoveButtonClick -= UnequipButtonClick;
        }

        private void OnEquipButtonClick(ILayoutElementData element)
        {
            ViewModel.OnEquipSkillGridElement(element);
        }

        private void UnequipButtonClick(ILayoutElementData element)
        {
            ViewModel.OnUnequipSkillGridElement(element);
        }

        private void OnSkillsChanged()
        {
            UpdateData();
            
            _upgradeButton.interactable = ViewModel.CanAnyUpgrade;
        }

        private void OnSelectedSkillGridElement(ILayoutElementData element)
        {
            ViewModel.SelectedSkillGridElement(element);
        }

        private void OnSelectedSkillPanelElement(SkillPanelViewData element)
        {
            ViewModel.OnSelectedSkillPanelElement(element);
        }

        private void UpdateData()
        {
            Logger.Warning($"skills quantity: {ViewModel.SkillsGridViewData.Values.Count}");
            
            _skillsPanel.Clear();
            _skillsPanel.SetData(ViewModel.SkillPanelViewData.Values);
            _skillGridWidget.Clear();
            _skillGridWidget.SetData(ViewModel.SkillsGridViewData.Values);

            _totalEffectWidget.SetValue(ViewModel.GetTotalEffect());
        }
    }
}