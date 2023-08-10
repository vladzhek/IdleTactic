using Slime.Data.Constants;
using Slime.UI.Common;
using Slime.UI.Common.Equipment;
using Slime.UI.Popups;
using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using Utils.Extensions;

namespace Slime.UI.Character.Skills
{
    public class EquipmentItemView : View<EquipmentItemViewModel>
    {
        [SerializeField] private PopupWidget _popupWidget;
        [SerializeField] private GridLayoutElement _element;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _activeAttack;
        [SerializeField] private TextMeshProUGUI _passiveAttack;
        
        [SerializeField] private GenericButton _equipButton;
        [SerializeField] private GenericButton _upgradeButton;
        
        public override UILayer Layer => UILayer.Overlay;

        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateState();
        }
        
        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            _popupWidget.OnCloseButtonClick += OnCloseButtonClicked;
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();

            _popupWidget.OnCloseButtonClick -= OnCloseButtonClicked;
        }

        private void OnCloseButtonClicked()
        {
            ViewModel.CloseView();
        }

        private void UpdateState()
        {
            var summonable = ViewModel.Data;
            _element.SetData(new GridLayoutData(summonable));
            
            _popupWidget.SetTile(summonable.Title);
            _descriptionText.text = summonable.Description;
            _activeAttack.text = Strings.ACTIVE_ATK.Resolve((summonable.ActiveValue * 100).ToMetricPrefixString());
            _passiveAttack.text = Strings.PASSIVE_ATK.Resolve((summonable.PassiveValue * 100).ToMetricPrefixString());
            
            _equipButton.Title = summonable.IsEquipped ? "Remove" : "Equip";
            _equipButton.Interactable = summonable.IsUnlocked;
            _upgradeButton.Interactable = summonable.CanUpgrade;
        }
    }
}