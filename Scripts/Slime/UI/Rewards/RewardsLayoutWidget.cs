using UI.Base.Widgets;

namespace Slime.UI.Rewards
{
    internal class RewardsLayoutWidget : LayoutWidget<RewardsLayoutElement, RewardsLayoutData>
    {
        protected override void OnElementSelect(RewardsLayoutElement layoutWidgetElement)
        {
            OnSelected(layoutWidgetElement.Data);
        }
    }
}