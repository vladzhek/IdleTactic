using Pooling;
using UnityEngine;
using Utils.Pooling;
using Utils.Promises;

namespace Slime.AbstractLayer
{
    public interface IEffect : IPoolObject
    {
        Transform Transform { get; }
        IEffect SetPosition(Vector3 position);
        IEffect SetRotation(Quaternion rotation);
        IEffect SetParent(Transform parent);
        IEffect SetDelay(float delay);
        IEffect SetLifetime(float lifetime);
        IEffect SetPromise(Promise promise);
    }
}