using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

namespace Slime.UI.Popups
{
    public class PopupWidget : MonoBehaviour
    {
        private const float MAX_HEIGHT = .8f;

        [SerializeField] private TMP_Text _title;
        [SerializeField] private LayoutElement _scrollLayoutElement;
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private Button _closeButton;
        
        public event Action OnCloseButtonClick;

        public void Resize()
        {
            _canvas ??= GetComponentInParent<Canvas>()?.transform as RectTransform;
            if (!_canvas)
            {
                return;
            }
            
            var canvasHeight = MAX_HEIGHT * _canvas.rect.height;
            var contentHeight = (_contentTransform as RectTransform)!.rect.height;
            
            var height = Mathf.Min(canvasHeight, contentHeight);
            //Logger.Log($"height: {height}");
            _scrollLayoutElement.minHeight = height;
            _scrollLayoutElement.preferredHeight = height;
            //_layoutElement.enabled = canvasHeight <= contentHeight;

            RequestRebuild();
        }

        public void SetTile(string text)
        {
            _title.text = text;
        }
        
        public void ShowCloseButton(bool isVisible)
        {
            _closeButton.gameObject.SetActive(isVisible);
        }
        
        public void SetScrollable(bool isActive)
        {
            _scrollLayoutElement.GetComponent<ScrollRect>().vertical = isActive;
        }

        // private
        
        private RectTransform _canvas;

        private void OnEnable()
        {
            _canvas ??= GetComponentInParent<Canvas>()?.transform as RectTransform;

            RequestRebuild();
            
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
        
        private void OnCloseButtonClicked()
        {
            OnCloseButtonClick?.Invoke();
        }
        
        private void RequestRebuild()
        {
            Invoke(nameof(Rebuild), .1f);
            //Invoke(nameof(Rebuild), .2f);
        }
        
        private void Rebuild()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentTransform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentTransform.parent as RectTransform);
        }
    }
}