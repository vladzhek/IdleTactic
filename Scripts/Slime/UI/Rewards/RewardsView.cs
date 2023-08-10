using Slime.UI.Popups;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;

namespace Slime.UI.Rewards
{
    public class RewardsView : View<RewardsViewModel>
    {
        [SerializeField] private PopupWidget _popupWidget;
        [SerializeField] private RewardsLayoutWidget _rewardLayoutWidget;
        
        public override UILayer Layer => UILayer.Overlay;
        
        #region View overrides
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _popupWidget.SetTile("REWARD");
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
            ViewModel.CloseView();
        }

        private void UpdateState()
        {
            _rewardLayoutWidget.SetData(ViewModel.Data);
        }
    }
}