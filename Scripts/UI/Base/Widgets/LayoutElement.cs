using System;
using UnityEngine;

namespace UI.Base.Widgets
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class LayoutElement<TElement, TData> : MonoBehaviour
        where TElement : LayoutElement<TElement, TData>
    {
        public event Action<TElement> OnSelect;

        [SerializeField] private RectTransform _container;
        [SerializeField] private bool _isAnimatedOnEnable;
        [SerializeField] private Vector3 _offset;

        private bool IsSelectable { get; set; } = true;
        public int Index { get; private set; }
        public TData Data { get; private set; }
        private CanvasGroup CanvasGroup { get; set; }

        private void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            GetComponent<RectTransform>();

            if (_container == null)
            {
                _container = GetComponent<RectTransform>();
                //Debug.LogWarning($"{name} has no container", gameObject);
            }
        }

        protected virtual void OnEnable()
        {
            //OffsetPosition();
        }
        
        protected virtual void OnDisable()
        {
            //LerpVisuals(1);
            //OffsetPosition();
        }

        public virtual void SetData(TData data)
        {
            if (Data?.Equals(data) ?? false)
            {
                //Logger.Warning($"{data} did not change");
                return;
            }
            
            Data = data;
        }

        public virtual void OnSelected()
        {
            if (IsSelectable)
            {
                OnSelect?.Invoke(this as TElement);
            }
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        public virtual void SetSelectable(bool isSelectable)
        {
            IsSelectable = isSelectable;
        }

        private void LerpVisuals(float step)
        {
            if (CanvasGroup)
            {
                CanvasGroup.alpha = step;
            }

            if (_container)
            {
                _container.localPosition = Vector3.Lerp(_offset, Vector3.zero, step);
            }
        }
        
        private void OffsetPosition()
        {
            if (_isAnimatedOnEnable)
            {
                _container.transform.localPosition = Vector3.up * 100;
            }
        }
    }
}