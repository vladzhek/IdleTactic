using Slime.Data.Enums;
using TMPro;
using UI.Base.Widgets;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Slime.UI.Popups.Summon
{
    public class SummonInfoElement : LayoutElement<SummonInfoElement, SummonInfoData>
    {
        public override void SetData(SummonInfoData data)
        {
            base.SetData(data);

            if (_rarityImage) _rarityImage.color = data.Rarity.ToColor();
            if (_percentageText) _titleText.text = data.Title;
            if (_percentageText) _percentageText.text = (data.Percentage * 100).ToMetricPrefixString() + "%";
        }
        
        // private

        [SerializeField] private Image _rarityImage;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _percentageText;
    }
}