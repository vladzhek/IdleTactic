using Slime.Data.Abstract;
using Slime.Data.Constants;
using Slime.Data.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Slime.UI.Common.Equipment
{
    public class EquipmentWidget : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _rarity;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        
        public void SetData(ILayoutElementData data)
        {
            if (_image) _image.sprite = data.Sprite;
            if (_rarity) _rarity.color = data.Rarity.ToColor();
            if (_level) _level.text = Strings.LEVEL.Resolve(data.Level);
            if (_title) _title.text = data.Title;
            if (_description) _description.text = data.Description;
        }
    }
}