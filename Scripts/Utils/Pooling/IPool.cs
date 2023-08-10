using UnityEngine;
using Utils.Pooling;

namespace Pooling
{
    public interface IPool
    {
        public T GetObject<T>(Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : IPoolObject;
        public void Release(PoolObject poolObject);
        public void Reset();
    }
}