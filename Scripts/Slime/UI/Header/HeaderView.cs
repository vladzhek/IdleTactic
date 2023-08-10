using JetBrains.Annotations;
using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace Slime.UI.Header
{
    public class HeaderView : View<HeaderViewModel>
    {
        [SerializeField] private TextMeshProUGUI _softCurrencyText;
        [SerializeField] private TextMeshProUGUI _hardCurrencyText;
        [SerializeField] private Button _buyHardCurrencyButton;
        [SerializeField, UsedImplicitly] private Button _settingsButton;

        public override UILayer Layer => UILayer.Foreground;

        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateState();
            _buyHardCurrencyButton.onClick.AddListener(OpenTabShop);
        }

        private void OpenTabShop()
        {
            ViewModel.OpenTabShop();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            ViewModel.OnCurrencyChange += OnCurrencyChanged;
        }
        
        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();

            ViewModel.OnCurrencyChange -= OnCurrencyChanged;
        }

        private void OnCurrencyChanged()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            _softCurrencyText.text = ViewModel.SoftCurrency;
            _hardCurrencyText.text = ViewModel.HardCurrency;
        }
    }
}