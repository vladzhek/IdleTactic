using Slime.UI.Common;
using TMPro;
using UI.Base.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Slime.UI.Store.Shop
{
    public class ShopLayoutElement : LayoutElement<ShopLayoutElement, ShopLayoutData>
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _purchaseBonus;
        [SerializeField] private GameObject _hotSale;
        [SerializeField] private GenericButton _buyButton;
        
        public override void SetData(ShopLayoutData data)
        {
            base.SetData(data);

            _titleText.text = data.Title;
            _quantityText.text = data.Quantity;
            _priceText.text = string.IsNullOrEmpty(data.Price) ? $"$0" : data.Price;
            _image.sprite = data.Sprite;
            _purchaseBonus.SetActive(data.IsBonus);
            _hotSale.SetActive(data.IsSale);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _buyButton.onClick.AddListener(OnSelected);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            _buyButton.onClick.RemoveListener(OnSelected);
        }
    }
}