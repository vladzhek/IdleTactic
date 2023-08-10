using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using Logger = Utils.Logger;

// ReSharper disable InconsistentNaming

namespace Slime.Data.Abstract
{
    [Serializable]
    public abstract class GenericData : IData, IClonable
    {
        public virtual string ID
        {
            get => _id;
            set => _id = value;
        }

        public virtual string SpriteID => ID;

        public virtual object Clone()
        {
            return ShallowCopy();
        }
        
        public override string ToString()
        { 
            return $"{base.ToString()}; id: {ID}";
        }
        
        // private
        
        [CanBeNull] protected string _id;

        private object ShallowCopy()
        {
            return MemberwiseClone();
        }
        
        private object DeepCopy()
        {
            Logger.Log($"object: {this}");
            //source.CopyPropertiesTo(result);
            //var json = JsonConvert.SerializeObject(this);
            //return JsonConvert.DeserializeObject<object>(json);
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<object>(json);
        }
    }
}