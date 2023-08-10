using Slime.Data.Abstract;
using Slime.UI.Abstract;
using Slime.UI.Common.Equipment;
using UI.Base.Widgets;

namespace Slime.UI.Character.Skills
{
    public class SkillWidget : LayoutWidget<GridLayoutElement,ILayoutElementData>
    {
        protected override void OnElementSelect(GridLayoutElement layoutWidgetElement)
        {
            OnSelected(layoutWidgetElement.Data);
        }
    }
}