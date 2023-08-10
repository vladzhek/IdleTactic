using System.Collections.Generic;
using Slime.AbstractLayer;
using UnityEngine;
using Zenject;

namespace Slime.Factories
{
    // NOTE: why not built in factories?
    public class ZenjectFactory : IObjectFactory
    {
        private readonly DiContainer _diContainer;

        public ZenjectFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public T CreateObject<T>()
        {
            return _diContainer.Instantiate<T>();
        }

        public T CreateObject<T>(IEnumerable<object> parameters)
        {
            return _diContainer.Instantiate<T>(parameters);
        }

        public T CreateObject<T>(T prefab) where T : Object
        {
            return _diContainer.InstantiatePrefabForComponent<T>(prefab);
        }

        public T CreateObject<T>(T prefab, Transform parent) where T : Object
        {
            return _diContainer.InstantiatePrefabForComponent<T>(prefab, parent);
        }

        public T CreateObject<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            return _diContainer.InstantiatePrefabForComponent<T>(prefab, position, rotation, parent);
        }
    }
}