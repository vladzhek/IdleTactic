using JetBrains.Annotations;
using Slime.UI.Common;
using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.UI.Popups.Dialogs
{
    public class ConfirmView : View<ConfirmViewModel>
    {
        [SerializeField] private PopupWidget _popupWidget;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private GameObject _buttonContainer;
        [SerializeField, UsedImplicitly] private GenericButton _confirmButton;
        [SerializeField, UsedImplicitly] private GenericButton _cancelButton;

        public override UILayer Layer => UILayer.Overlay;
        
        #region View overrides
        
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

        #endregion

        private void OnCloseButtonClicked()
        {
            ViewModel.RequestClose();
        }
        
        [UsedImplicitly]
        private void OnConfirmButtonClicked()
        {
            ViewModel.RequestConfirm();
        }
        
        [UsedImplicitly]
        private void OnCancelButtonClicked()
        {
            ViewModel.RequestCancel();
        }
        
        private void UpdateState()
        {
            var data = ViewModel.Data;
            
            _popupWidget.SetTile(data.Title);
            _popupWidget.ShowCloseButton(!data.CanCancel);
            _description.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(data.Description));
            _description.text = data.Description ?? "";
            _buttonContainer.SetActive(data.CanCancel);
            _confirmButton.Title = data.ConfirmTitle;
            _cancelButton.Title = data.CancelTitle;
        }
    }
}