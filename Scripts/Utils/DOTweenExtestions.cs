using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class DOTweenExtestions
    {
        public static Tweener DOMoveToTarget(this Transform transform, Vector3 target, float time = 1)
        {
            var initialPosition = transform.position;

            return DOVirtual.Float(0, 1, time, t => { transform.position = Vector3.Lerp(initialPosition, target, t); });
        }
    }
}