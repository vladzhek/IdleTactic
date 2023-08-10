using UI.Base.Widgets;

namespace Slime.UI.Boosters
{
    internal class BoostersLayout : LayoutWidget<BoosterLayoutElement,BoosterLayoutElementViewData>
    {
        protected override void OnElementSelect(BoosterLayoutElement layoutLayoutElement)
        {
            OnSelected(layoutLayoutElement.Data);
        }
    }
}