using System;
using Pooling;
using UnityEngine;
using UnityEngine.Pool;

namespace Utils.Pooling
{
    public class UnityPool : IPool
    {
        private readonly ObjectPool<PoolObject> _pool;

        public UnityPool(Func<PoolObject> creationMethod, int capacity = 10)
        {
            _pool = new ObjectPool<PoolObject>(
                creationMethod,
                poolObject => poolObject.OnExtractFromPool(),
                defaultCapacity: capacity);
        }

        public T GetObject<T>(Vector3 position, Quaternion rotation, Transform parent = null) where T : IPoolObject
        {
            var poolObject = _pool.Get();

            poolObject.Transform.SetParent(parent);
            poolObject.Transform.position = position;
            poolObject.Transform.rotation = rotation;

            poolObject.ReturnToPool += OnObjectReturnToPool;

            if (poolObject is T typedPoolObject)
            {
                return typedPoolObject;
            }

            Debug.LogError($"Couldn't convert {poolObject.GetType()} to {typeof(T)}");
            return default;
        }

        public T GetObject<T>(Vector3 position = default) where T : IPoolObject
        {
            var poolObject = _pool.Get();
            poolObject.Transform.position = position;

            poolObject.ReturnToPool += OnObjectReturnToPool;

            if (poolObject is T typedPoolObject)
            {
                return typedPoolObject;
            }

            Debug.LogError($"Couldn't convert {poolObject.GetType()} to {typeof(T)}");
            return default;
        }

        public void Release(PoolObject poolObject)
        {
            poolObject.ReturnToPool -= OnObjectReturnToPool;
            _pool.Release(poolObject);
        }

        private void OnObjectReturnToPool(IPoolObject obj)
        {
            Release(obj as PoolObject);
        }

        public void Reset()
        {
            _pool.Clear();
        }
    }
}