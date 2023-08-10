using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Slime.UI.Common
{
    // DEBUG: opened prefab in play mode
    //[ExecuteInEditMode]
    public class GenericButton : Button
    {
        [Header("Generic button")]
        [SerializeField] private Image _image;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Sprite _backgroundSprite;
        [SerializeField] private Sprite _backgroundLockedSprite;
        [SerializeField] private Sprite _iconSprite;
        [SerializeField] private Sprite _iconLockedSprite;
        [SerializeField] private TextMeshProUGUI _title;

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once MemberCanBePrivate.Global
        public new bool interactable
        {
            get => base.interactable;
            set { base.interactable = value; UpdateState(); }
        }
        
        // ReSharper disable once UnusedMember.Global
        public bool Interactable
        {
            get => interactable;
            set => interactable = value;
        }

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set { _selected = value; UpdateState(); }
        }

        public string Title
        {
            get => _title ? _title.text : null;
            set => _title.text = value;
        }
        
        public virtual Sprite Sprite
        {
            get => _backgroundSprite;
            set { _backgroundSprite = value; UpdateState(); }
        }
        
        public Sprite IconSprite
        {
            get => _iconSprite;
            set { _iconSprite = value; UpdateState(); }
        }

        protected override void Awake()
        {
            base.Awake();

            // TODO: do this in editor automatically
            
            if (!_image) _image = (Image)targetGraphic;
            if (!_backgroundSprite && _image) _backgroundSprite = _image.sprite;
            if (!_iconSprite && _icon) _iconSprite = _icon.sprite;

            UpdateState();
        }
        
        protected virtual void UpdateState()
        {
            //Logger.Log($"interactable {interactable} icon sprite {(_icon ? _icon.sprite : null)}");
            
            if (interactable)
            {
                if (_image) _image.sprite = _backgroundSprite;
                if (_icon) _icon.sprite = _iconSprite;
            }
            else
            {
                if (_image && _backgroundLockedSprite) _image.sprite = _backgroundLockedSprite;
                if (_icon && _iconLockedSprite) _icon.sprite = _iconLockedSprite;
            }

            if (_icon)
            {
                _icon.gameObject.SetActive(_icon.sprite);
                _icon.transform.parent.gameObject.SetActive(_icon.sprite);
            }
            if (_selection) _selection.gameObject.SetActive(_selected);
        }

        /*
        protected override void OnValidate()
        {
            base.OnValidate();

            UpdateState();
        }
        */
    }
}