using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Logger = Utils.Logger;

namespace Slime.UI.Common
{
    public class ContinuousClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private AnimationCurve _callbackFrequencyCurve;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _disabledSprite;
        [SerializeField] private float _bounceForce;
        [SerializeField] private Ease _ease;

        [SerializeField] private UnityEvent _unityEvent;

        public bool IsInteractable { get; private set; }

        private Tweener _animation;
        private Vector3 _scale;
        private Sprite _imageSprite;

        private bool _isPressed;
        private bool _inRefraction;

        private void Awake()
        {
            _imageSprite = _image.sprite;
            _scale = transform.localScale;
        }

        public void AddListener(UnityAction action)
        {
            _unityEvent.AddListener(action);
        }

        public void RemoveListener(UnityAction action)
        {
            _unityEvent.RemoveListener(action);
        }

        public void SetInteractable(bool isInteractable)
        {
            _image.sprite = isInteractable ? _imageSprite : _disabledSprite;
            IsInteractable = isInteractable;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Logger.Log("Click");
            _isPressed = true;
            StartCoroutine(Press());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
        }

        private IEnumerator Press()
        {
            var i = 0;
            while (_isPressed)
            {
                //Logger.Log($"cycle {i} {gameObject.name}");

                OnPress();

                yield return new WaitForSeconds(_callbackFrequencyCurve.Evaluate(i++));
            }
        }

        private void OnPress()
        {
            if (!IsInteractable)
            {
                return;
            }

            _unityEvent?.Invoke();
            //Logger.Log($"Invoke on {gameObject.name}");
            _animation?.Kill(true);
            _animation = transform.DOPunchScale(_scale * _bounceForce, .1f).SetEase(_ease);
        }
    }
}