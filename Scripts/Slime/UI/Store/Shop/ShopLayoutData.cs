using Slime.Data.Abstract;
using UI.Base.Widgets;
using UnityEngine;
using Utils.Extensions;

namespace Slime.UI.Store.Shop
{
    public class ShopLayoutData : 
        LayoutData<ShopLayoutData>
    {
        // TODO: move to layout data
        // TODO: remove base layout data
        // TODO: add Title to layout data
        public readonly string ID;
        public readonly string Quantity;
        public readonly string Price;
        public readonly bool IsSale;
        public readonly bool IsBonus;

        public ShopLayoutData(IProduct data, Sprite sprite = null) : 
            base(data.Title, sprite != null ? sprite : data.Sprite)
        {
            ID = data.ID;
            Quantity = data.Resource.Quantity.ToMetricPrefixString();
            Price = data.LocalizedPrice;
            IsSale = data.IsSale;
            IsBonus = data.IsBonus;
        }
        
        protected override bool IsEqualTo(ShopLayoutData other) =>
            base.IsEqualTo(other)
            && ID == other.ID
            && Quantity == other.Quantity
            && Price == other.Price
            && IsSale == other.IsSale
            && IsBonus == other.IsBonus
            ;

        protected override int HashCode => System.HashCode.Combine(
            base.HashCode,
            ID,
            Quantity,
            Price,
            IsSale,
            IsBonus
            );
    }
}