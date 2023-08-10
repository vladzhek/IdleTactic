using UI.Base.Widgets;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.UI.Common.Abilities
{
    public class AbilityElement : LayoutElement<AbilityElement, AbilityLayoutData>
    {
        [SerializeField] private GenericButton _button;
        
        public override void SetData(AbilityLayoutData layoutData)
        {
            base.SetData(layoutData);

            if (!_button)
            {
                Logger.Warning($"button not set");
                return;
            }

            if (layoutData.Sprite) _button.Sprite = layoutData.Sprite;
        }
        
        protected override void OnEnable()
        {
            base.OnDisable();
            
            if (_button) _button.onClick.AddListener(OnSelected);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (_button) _button.onClick.RemoveListener(OnSelected);
        }
    }
}