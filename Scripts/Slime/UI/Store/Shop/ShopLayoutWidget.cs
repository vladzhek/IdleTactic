using UI.Base.Widgets;

namespace Slime.UI.Store.Shop
{
    internal class ShopLayoutWidget : LayoutWidget<ShopLayoutElement, ShopLayoutData>
    {
        protected override void OnElementSelect(ShopLayoutElement layoutWidgetElement)
        {
            OnSelected(layoutWidgetElement.Data);
        }
    }
}