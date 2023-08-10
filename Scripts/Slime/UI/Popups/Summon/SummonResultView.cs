using System.Linq;
using Slime.UI.Common.Equipment;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;

namespace Slime.UI.Popups.Summon
{
    public class SummonResultView: View<SummonResultViewModel>
    {
        [SerializeField] private PopupWidget _popupWidget;
        [SerializeField] private EquipmentLayout _equipmentLayout;
        
        public override UILayer Layer => UILayer.Overlay;

        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateState();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            _popupWidget.OnCloseButtonClick += ViewModel.CloseView;
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            _popupWidget.OnCloseButtonClick -= ViewModel.CloseView;
        }

        private void UpdateState()
        {
            // NOTE: ToTitle()?
            _popupWidget.SetTile(ViewModel.Type.ToString());
            _equipmentLayout.SetData(from item in ViewModel.Data 
                select new GridLayoutData(item));

            RequestResize();
        }

        private void RequestResize()
        {
            Invoke(nameof(Resize), .1f);
        }
        
        private void Resize()
        {
            _popupWidget.Resize();
        }
    }
}