using DG.Tweening;
using Pooling;
using Slime.AbstractLayer;
using TMPro;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Utils.Pooling;

namespace Slime.UI
{
    public class DamageText : PoolObject, IUpdatable
    {
        [SerializeField] private TextMeshProUGUI _textField;
        [SerializeField] private float _distance;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        private Vector3 _from;
        private Vector3 _to;
        private float _time;

        public override void OnExtractFromPool()
        {
            base.OnExtractFromPool();

            _time = 0;
            _textField.alpha = 1;
        }

        public void SetValue(Vector3 position, float value)
        {
            _from = position;
            _to = _from + Vector3.up * _distance;
            _textField.text = ((int)value).ToMetricPrefixString();
        }

        public void OnUpdate(float deltaTime)
        {
            _time += deltaTime;

            transform.position = DOVirtual.EasedValue(_from, _to, _time / _duration, _ease);
            _textField.alpha = DOVirtual.EasedValue(1, 0, _time / _duration, _ease);
            if (_time >= _duration)
            {
                Release();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(_from, _to);
        }
    }
}