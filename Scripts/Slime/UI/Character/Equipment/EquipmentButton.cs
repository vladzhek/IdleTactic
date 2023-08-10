using Slime.Data.Constants;
using Slime.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Slime.UI.Character.Equipment
{
    public class EquipmentButton : GenericButton
    {
        [SerializeField] private Image _rarityImage;
        public Color RarityColor
        {
            set => _rarityImage.color = value;
        }

        [SerializeField] private TextMeshProUGUI _levelText;
        private int _level;
        public int Level
        {
            get => _level;
            set { _level = value; UpdateState(); }
        }

        [SerializeField] private Sprite _defaultSprite;
        public override Sprite Sprite
        {
            get => base.Sprite;
            set => base.Sprite = value ? value : _defaultSprite;
        }
        
        protected override void UpdateState()
        {
            base.UpdateState();

            if (_levelText)
            {
                _levelText.transform.parent.gameObject.SetActive(Level > 0);
                _levelText.text = Strings.LEVEL.Resolve(Level);
            }
        }
    }
}