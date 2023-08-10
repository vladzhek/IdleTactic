using System;

namespace Utils.Pooling
{
    public interface IPoolObject
    {
        event Action<IPoolObject> ReturnToPool;
        event Action<IPoolObject> Extracted;

        void OnExtractFromPool();
        void Release();
    }
}