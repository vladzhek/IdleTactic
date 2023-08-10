using System;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Data.Constants;
using Slime.Data.Enums;
using Slime.UI.Popups.Settings;
using UI.Base.MVVM;
using Utils.Extensions;

namespace Slime.UI.Header
{
    public class HeaderViewModel : ViewModel
    {
        public event Action OnCurrencyChange;
        public string SoftCurrency => _resourcesModel.Get(EResource.SoftCurrency).ToMetricPrefixString();
        public string HardCurrency => _resourcesModel.Get(EResource.HardCurrency).ToMetricPrefixString();

        public override void OnSubscribe()
        {
            base.OnSubscribe();
            
            _resourcesModel.OnChange += OnResourceChanged;
        }

        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            _resourcesModel.OnChange -= OnResourceChanged;
        }

        // private
        
        private readonly IResourcesModel _resourcesModel;
        private readonly UIManager _uiManager;
        private readonly IFooterUIModel _footerUIModel;
        private readonly ICharacterUIModel _characterUIModel;

        public HeaderViewModel(IResourcesModel resourcesModel, 
            UIManager uiManager, 
            ICharacterUIModel characterUIModel, 
            IFooterUIModel footerUIModel)
        {
            _resourcesModel = resourcesModel;
            _uiManager = uiManager;
            _footerUIModel = footerUIModel;
            _characterUIModel = characterUIModel;
        }

        private void OnResourceChanged(EResource resource, float quantity)
        {
            if (resource is EResource.SoftCurrency or EResource.HardCurrency)
            {
                OnCurrencyChange?.Invoke();
            }
        }

        [UsedImplicitly]
        private void OnSettingsButtonClicked()
        {
            _uiManager.Open<SettingsView>();
        }

        public void OpenTabShop()
        {
            _footerUIModel.SelectTab(Values.FOOTER_TAB_STORE);
            _characterUIModel.SelectTab(Values.STORE_TAB_SHOP);
        }
    }
}