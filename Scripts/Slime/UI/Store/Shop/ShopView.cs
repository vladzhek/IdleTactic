using UI.Base;
using UI.Base.MVVM;
using UnityEngine;

namespace Slime.UI.Store.Shop
{
    public class ShopView : View<ShopViewModel>
    {
        [SerializeField] private ShopLayoutWidget _shopLayoutWidget;
        
        public override UILayer Layer => UILayer.StoreTabbar;
        
        #region View overrides
        
        protected override void Start()
        {
            base.Start();
            
            _shopLayoutWidget.Clear();
            UpdateState();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            ViewModel.OnItemChange += OnItemChanged;
            _shopLayoutWidget.OnSelect += OnItemSelected;
        }
        
        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            ViewModel.OnItemChange -= OnItemChanged;
            _shopLayoutWidget.OnSelect -= OnItemSelected;
        }

        #endregion
        
        private void UpdateState()
        {
            _shopLayoutWidget.SetData(ViewModel.Get());
        }
        
        private void OnItemSelected(ShopLayoutData data)
        {
            ViewModel.PurchaseItem(data.ID);
        }
        
        private void OnItemChanged(int index, ShopLayoutData data)
        {
            _shopLayoutWidget.SetData(index, data);
        }
    }
}