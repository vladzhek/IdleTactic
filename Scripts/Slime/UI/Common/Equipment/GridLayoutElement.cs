using System;
using Slime.Data.Abstract;
using Slime.Data.Constants;
using Slime.Data.Enums;
using TMPro;
using UI.Base.Widgets;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Slime.UI.Common.Equipment
{
    public class GridLayoutElement : LayoutElement<GridLayoutElement, ILayoutElementData>
    {
        [SerializeField] private GameObject _lockedBackground;
        [SerializeField] private GameObject _equippedBackground;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _sliderText;
        [SerializeField] private Image _rarityBackground;
        [SerializeField] private Image _mainIconImage;
        [SerializeField] private Image _attentionIconImage;

        [SerializeField] private GameObject _buttonsContainer;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Button _selectButton;
        [SerializeField] private RectTransform _tutorialFinger;

        public bool CanEquip
        {
            set
            {
                if (_buttonsContainer) _buttonsContainer.SetActive(value);
            }
        }

        public event Action<ILayoutElementData> OnAddButtonClick;
        public event Action<ILayoutElementData> OnRemoveButtonClick;
        
        public override void SetData(ILayoutElementData data)
        {
            base.SetData(data);

            _rarityBackground.color = data.Rarity.ToColor();
            _mainIconImage.sprite = data.Sprite;

            if (_attentionIconImage) _attentionIconImage.gameObject.SetActive(data.NeedsAttention);  
            
            if (_lockedBackground) _lockedBackground.SetActive(!data.IsUnlocked);
            if (_equippedBackground) _equippedBackground.SetActive(data.IsEquipped);
            if (_levelText) _levelText.text = Strings.LEVEL.Resolve(data.Level); //$"Lv {data.Level:D3}";
            if (_levelText) _levelText.transform.parent.gameObject.SetActive(data.IsUnlocked);
            if (_slider) _slider.value = data.Progress;
            if (_sliderText) _sliderText.text = $"{data.Quantity}/{data.UpgradeQuantity}";
            
            if (_addButton) _addButton.gameObject.SetActive(!data.IsEquipped);
            if (_removeButton) _removeButton.gameObject.SetActive(data.IsEquipped);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _selectButton.onClick.AddListener(OnSelectButtonClicked);
            
            if (_addButton) _addButton.onClick.AddListener(OnAddButtonClicked);
            if (_removeButton) _removeButton.onClick.AddListener(OnRemoveButtonClicked);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            _selectButton.onClick.RemoveListener(OnSelectButtonClicked);
            
            if (_addButton) _addButton.onClick.RemoveListener(OnAddButtonClicked);
            if (_removeButton) _removeButton.onClick.RemoveListener(OnRemoveButtonClicked);
        }

        private void OnAddButtonClicked()
        {
            OnAddButtonClick?.Invoke(Data);
        }
        
        private void OnRemoveButtonClicked()
        {
            OnRemoveButtonClick?.Invoke(Data);
        }
        
        private void OnSelectButtonClicked()
        {
            OnSelected();
        }

        public void SetTutorialFingerActive(bool isActive)
        {
            _tutorialFinger.gameObject.SetActive(isActive);
        }
    }
}