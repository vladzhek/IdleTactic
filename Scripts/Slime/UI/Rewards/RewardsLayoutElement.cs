using Slime.AbstractLayer.Models;
using TMPro;
using UI.Base.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Slime.UI.Rewards
{
    public class RewardsLayoutElement : LayoutElement<RewardsLayoutElement, RewardsLayoutData>
    {
        private ISpritesModel _spritesModel;
        
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _quantity;

        public override void SetData(RewardsLayoutData data)
        {
            base.SetData(data);
            
            _image.sprite = data.Sprite;
            _quantity.text = data.Quantity;
        }
    } 
}