using Slime.Data.Constants;
using TMPro;
using UnityEngine;
using Utils.Extensions;

namespace Slime.UI.Common
{
    public class TotalEffectWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;

        public void SetValue(float value, string template = Strings.ATK)
        {
            _valueText.text = template.Resolve(value.ToMetricPrefixString());
        }
    }
}