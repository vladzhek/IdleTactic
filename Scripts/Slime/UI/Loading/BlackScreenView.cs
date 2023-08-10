using DG.Tweening;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace Slime.UI
{
    public class BlackScreenView : View<BlackScreenViewModel>
    {
        [SerializeField] private Image _image;

        private Tweener _fadeTween;

        public override UILayer Layer => UILayer.BlackScreen;

        protected override void OnEnable()
        {
            base.OnEnable();

            OnActivityChanged(ViewModel.IsActive.Value);
            ViewModel.IsActive.Changed += OnActivityChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            ViewModel.IsActive.Changed -= OnActivityChanged;
        }

        private void OnActivityChanged(bool isActive)
        {
            _image.raycastTarget = true;
            
            var target = isActive ? 1 : 0;
            
            _fadeTween?.Kill();
            _fadeTween = _image.DOFade(target, 1).OnComplete(() => _image.raycastTarget = isActive);
        }
    }
}