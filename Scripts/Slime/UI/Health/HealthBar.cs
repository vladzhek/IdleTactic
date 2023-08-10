using Pooling;
using UnityEngine;
using UnityEngine.UI;
using Utils.Pooling;

namespace Slime.UI
{
    public class HealthBar : PoolObject
    {
        [SerializeField] private Slider _slider;

        public void SetValue(float value)
        {
            _slider.value = value;
        }
    }
}