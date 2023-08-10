using System;
using Pooling;
using UnityEngine;

namespace Utils.Pooling
{
    public class PoolObject : MonoBehaviour, IPoolObject
    {
        public event Action<IPoolObject> ReturnToPool;
        public event Action<IPoolObject> Extracted;

        public Transform Transform => transform;

        public virtual void OnExtractFromPool()
        {
            gameObject.SetActive(true);
            Extracted?.Invoke(this);
        }

        public virtual void Release()
        {
            ReturnToPool?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}