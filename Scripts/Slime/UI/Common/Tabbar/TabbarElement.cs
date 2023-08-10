using UI.Base.Widgets;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.UI.Common.Tabbar
{
    public class TabbarElement : LayoutElement<TabbarElement, TabbarData>
    {
        [SerializeField] private GenericButton _button;
        
        public override void SetData(TabbarData data)
        {
            base.SetData(data);

            //Logger.Log($"data {data}");
            
            if (!_button)
            {
                Logger.Warning($"button not set");
                return;
            }

            if (data.Sprite) _button.IconSprite = data.Sprite;
            _button.Interactable = data.IsUnlocked;
            _button.Selected = data.IsSelected;
            _button.Title = data.Title;
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