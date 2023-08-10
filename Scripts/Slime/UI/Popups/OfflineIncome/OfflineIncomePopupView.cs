using Slime.UI.Common;
using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace Slime.UI.Popups
{
    public class OfflineIncomePopupView : View<OfflineIncomeViewModel>
    {
        [SerializeField] private PopupWidget _popupWidget;
        [SerializeField] private TextMeshProUGUI _rewardAmount;
        [SerializeField] private GenericButton _adButton;

        public override UILayer Layer => UILayer.Overlay;

        protected override void OnEnable()
        {
            base.OnEnable();

            _popupWidget.SetTile("Offline income");
            _popupWidget.OnCloseButtonClick += CloseView;
            _rewardAmount.text = $"{ViewModel.GetRewardValue()}";
            _adButton.onClick.AddListener(ShowAd);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _popupWidget.OnCloseButtonClick -= CloseView;
            _adButton.onClick.RemoveListener(ShowAd);
        }
        
        private void CloseView()
        {
            ViewModel.CloseView();
        }
        
        private void ShowAd()
        {
            ViewModel.ShowAd();
        }
    }
}