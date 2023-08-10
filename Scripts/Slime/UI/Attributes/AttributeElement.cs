using Slime.UI.Common;
using TMPro;
using UI.Base.Widgets;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

namespace Slime.UI.Attributes
{
    public class AttributeElement : LayoutElement<AttributeElement, AttributeData>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _attributeName;
        [SerializeField] private TextMeshProUGUI _attributeValue;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private GameObject _tutorialFinger;

        [SerializeField] private ContinuousClickButton _buyButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            _buyButton.AddListener(OnSelected);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _buyButton.RemoveListener(OnSelected);
        }

        public override void SetData(AttributeData data)
        {
            base.SetData(data);

            _icon.sprite = data.Sprite;
            _attributeName.text = data.Title;
            _attributeValue.text = data.Value;
            _level.text = data.Level;
            _cost.text = data.Cost;

            _buyButton.SetInteractable(data.CanUpgrade);
        }

        public void SetActiveTutorialFinger(bool isActive)
        {
            _tutorialFinger.SetActive(isActive);
        }
    }
}