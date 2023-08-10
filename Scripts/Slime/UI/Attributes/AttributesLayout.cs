using UI.Base.Widgets;

namespace Slime.UI.Attributes
{
    internal class AttributesLayout : LayoutWidget<AttributeElement, AttributeData>
    {
        protected override void OnElementSelect(AttributeElement layoutWidgetElement)
        {
            OnSelected(layoutWidgetElement.Data);
        }
    }
}