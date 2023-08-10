using System;
using Slime.Data.Abstract;
using UI.Base.Widgets;
using UnityEngine;

namespace Slime.UI.Common.Equipment
{
    internal class EquipmentLayout : LayoutWidget<GridLayoutElement,ILayoutElementData>
    {
        [SerializeField] private bool _canEquip = true; 
        
        public event Action<ILayoutElementData> OnAddButtonClick;
        public event Action<ILayoutElementData> OnRemoveButtonClick;

        protected override void OnElementSelect(GridLayoutElement layoutWidgetElement)
        {
            OnSelected(layoutWidgetElement.Data);
        }

        protected override void Subscribe(GridLayoutElement element)
        {
            // TODO: check that subscribe is called once
            base.Subscribe(element);

            element.CanEquip = _canEquip;
            
            if (_canEquip)
            {
                element.OnAddButtonClick += OnAddButtonClicked;
                element.OnRemoveButtonClick += OnRemoveButtonClicked;
            }
        }
        
        protected override void Unsubscribe(GridLayoutElement element)
        {
            base.Subscribe(element);
            
            if (_canEquip)
            {
                element.OnAddButtonClick -= OnAddButtonClicked;
                element.OnRemoveButtonClick -= OnRemoveButtonClicked;
            }
        }

        private void OnAddButtonClicked(ILayoutElementData data)
        {
            OnAddButtonClick?.Invoke(data);
        }
        
        private void OnRemoveButtonClicked(ILayoutElementData data)
        {
            OnRemoveButtonClick?.Invoke(data);
        }
    }
}