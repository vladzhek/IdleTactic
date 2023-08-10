using System;
using System.Collections.Generic;

namespace UI.Base
{
    public static class ModelRepository
    {
        private static Dictionary<Type, Model> Models = new Dictionary<Type, Model>();

        public static T Resolve<T>() where T : Model, new()
        {
            if(!Models.TryGetValue(typeof(T), out var model))
            {
                model = new T();
                Models.Add(typeof(T), model);
            }

            return model as T;
        }
    }
}