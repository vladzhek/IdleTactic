using Slime.Data.Skills;
using UI.Base.Widgets;

namespace Slime.UI.Battle
{
    public class SkillsPanel : LayoutWidget<SkillPanelElement, SkillPanelViewData>
    {
        protected override void OnElementSelect(SkillPanelElement layoutWidgetElement)
        {
            OnSelected(layoutWidgetElement.Data);
        }
    }
}