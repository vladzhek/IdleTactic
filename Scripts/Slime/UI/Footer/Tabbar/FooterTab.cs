using UI.Base.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Slime.UI.Footer.Tabbar
{
    public class FooterTab : LayoutElement<FooterTab, FooterLayoutData>
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _closeButton;
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _lock;
        [SerializeField] private GameObject _tutorialFinger;

        public override void SetData(FooterLayoutData data)
        {
            base.SetData(data);

            _image.sprite = data.Sprite;
            _closeButton.gameObject.SetActive(data.IsSelected);
            _content.gameObject.SetActive(!data.IsSelected);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            
            SetTutorialActive(false);
        }

        public override void SetSelectable(bool isSelectable)
        {
            base.SetSelectable(isSelectable);
            
            _lock.SetActive(!isSelectable);
        }

        public void SetTutorialActive(bool isActive)
        {
            _tutorialFinger.SetActive(isActive);
        }
    }
}