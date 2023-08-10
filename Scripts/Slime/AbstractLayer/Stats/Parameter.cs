using System;
using UnityEngine;
using Utils.Promises;

namespace Slime.AbstractLayer.Stats
{
    /// <summary>
    /// Flexible value that changed in game, used for tracking unit's current state
    /// Example : Health = current value (10), Promised value (8), because of delayed damage
    /// </summary>
    [Serializable]
    public class Parameter : IDisposable
    {
        public event Action<Parameter, float> Changed;

        public string ID { get; }
        [field: SerializeField] public Attribute Attribute { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        [field: SerializeField] public float PromisedValue { get; private set; }

        public Parameter(Attribute attribute)
        {
            Attribute = attribute;
            attribute.Changed += OnAttributeChanged;
            ID = attribute.ID;
            Value = attribute.FinalValue;
            PromisedValue = attribute.FinalValue;
        }

        public Promise Change(float modification)
        {
            PromisedValue += modification;
            PromisedValue = Math.Clamp(PromisedValue, 0, Attribute.FinalValue);
            return new Promise(() =>
            {
                if (Attribute == null)
                {
                    return;
                }

                var newValue = Value + modification;
                newValue = Math.Clamp(newValue, 0, Attribute.FinalValue);
                if (Math.Abs(Value - newValue) < float.Epsilon)
                {
                    return;
                }

                Value = newValue;
                Changed?.Invoke(this, modification);
            });
        }

        public void Reset()
        {
            Value = Attribute.FinalValue;
            PromisedValue = Value;
            Changed?.Invoke(this, 0);
        }

        private void OnAttributeChanged(Attribute attribute)
        {
            Value = Math.Clamp(Value, 0, attribute.FinalValue);
            PromisedValue = Math.Clamp(PromisedValue, 0, attribute.FinalValue);
        }

        public void Dispose()
        {
            Attribute.Changed -= OnAttributeChanged;
            Attribute = null;
        }
    }
}