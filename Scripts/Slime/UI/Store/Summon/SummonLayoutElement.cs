using System;
using Slime.Data.Constants;
using Slime.Data.Enums;
using Slime.UI.Common;
using TMPro;
using UI.Base.Widgets;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Slime.UI.Store.Summon
{
    public class SummonLayoutElement : LayoutElement<SummonLayoutElement, SummonLayoutElementData>
    {
        public event Action<ESummonType> OnInfoButtonClick;
        public event Action<ESummonType> OnAdButtonClick;
        public event Action<ESummonType> OnLowQuantitySummonButtonClick;
        public event Action<ESummonType> OnHighQuantitySummonButtonClick;

        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _sliderText;
        [SerializeField] private Slider _slider;
        [SerializeField] private Button _infoButton;

        [SerializeField] private TMP_Text _lowQuantitySummonText;
        [SerializeField] private GenericButton _lowQuantitySummonButton;

        [SerializeField] private TMP_Text _highQuantitySummonText;
        [SerializeField] private GenericButton _highQuantitySummonButton;
        
        [SerializeField] private TMP_Text _adSummonQuantityText;
        [SerializeField] private TMP_Text _adRemainingViewsQuantityText;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private GenericButton _adButton;
        
        [SerializeField] private RectTransform _tutorialFinger;

        public override void SetData(SummonLayoutElementData data)
        {
            base.SetData(data);
            
            //Logger.Log($"data: {data}");

            _levelText.text = Strings.LEVEL.Resolve(data.Level);
            _sliderText.text = $"{data.Quantity}/{data.UpgradeQuantity}";
            _slider.value = data.Progress;
            
            _lowQuantitySummonText.text = $"{data.LowSummonQuantity}";
            _lowQuantitySummonButton.Interactable = data.CanSummonLow;
            _lowQuantitySummonButton.Title = data.LowSummonTitle;

            _highQuantitySummonText.text = $"{data.HighSummonQuantity}";
            _highQuantitySummonButton.Interactable = data.CanSummonHigh;
            _highQuantitySummonButton.Title = data.HighSummonTitle;
            
            _adButton.Interactable = data.AdRemainingViewsQuantity > 0;
            _adSummonQuantityText.text = $"{data.AdSummonQuantity}/{data.AdMaxSummonQuantity}";
            _adRemainingViewsQuantityText.text = $"{data.AdRemainingViewsQuantity}/{data.AdMaxViewsQuantity}";
            _timerText.transform.parent.gameObject.SetActive(data.AdRemainingViewsQuantity != data.AdMaxViewsQuantity);
        }

        public void SetTimerText(string timerText)
        {
            _timerText.text = timerText;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _infoButton.onClick.AddListener(OnInfoButtonClicked);
            _adButton.onClick.AddListener(OnAdButtonClicked);
            _lowQuantitySummonButton.onClick.AddListener(OnLowQuantitySummonButtonClicked);
            _highQuantitySummonButton.onClick.AddListener(OnHighQuantitySummonButtonClicked);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            _infoButton.onClick.RemoveListener(OnInfoButtonClicked);
            _adButton.onClick.RemoveListener(OnAdButtonClicked);
            _lowQuantitySummonButton.onClick.RemoveListener(OnLowQuantitySummonButtonClicked);
            _highQuantitySummonButton.onClick.RemoveListener(OnHighQuantitySummonButtonClicked);
        }

        private void OnInfoButtonClicked()
        {
            OnInfoButtonClick?.Invoke(Data.Type);
        }
        
        private void OnAdButtonClicked()
        {
            OnAdButtonClick?.Invoke(Data.Type);
        }
        
        private void OnLowQuantitySummonButtonClicked()
        {
            OnLowQuantitySummonButtonClick?.Invoke(Data.Type);
        }
        
        private void OnHighQuantitySummonButtonClicked()
        {
            OnHighQuantitySummonButtonClick?.Invoke(Data.Type);
        }

        public void SetTutorialActive(bool isActive)
        {
            _tutorialFinger.gameObject.SetActive(isActive);
        }
    }
}