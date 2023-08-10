using ModestTree;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.UI.Popups.Summon
{
    public class SummonInfoView : View<SummonInfoViewModel>
    {
        public override UILayer Layer => UILayer.Overlay;
        
        // private

        [SerializeField] private PopupWidget _popupWidget;
        [SerializeField] private SummonInfoWidget _summonInfoWidget;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _summonInfoWidget.SetData(ViewModel.Data);
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
    }
}