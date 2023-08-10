using System.Collections.Generic;
using UnityEngine;

namespace Slime.AbstractLayer
{
    public interface IObjectFactory
    {
        public T CreateObject<T>();
        public T CreateObject<T>(IEnumerable<object> parameters);
        public T CreateObject<T>(T prefab) where T : Object;
        public T CreateObject<T>(T prefab, Transform parent) where T : Object;
        public T CreateObject<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object;
    }
}