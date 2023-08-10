using System;
using Slime.Data;
using Slime.Data.Enums;

namespace Slime.AbstractLayer.Stats
{
    [Serializable]
    public class Modifier
    {
        public string ID { get; }
        public float Value { get; }
        public EModificationType Type { get; }

        public Modifier(string id, float value, EModificationType type = EModificationType.Multiply)
        {
            ID = id;
            Value = value;
            Type = type;
        }
    }
}