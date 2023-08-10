using System;
using System.Collections.Generic;
using System.Linq;
using Slime.Data.Enums;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.AbstractLayer.Stats
{
    /// <summary>
    /// Modifiable value based on config
    /// Example : Damage = base (10) + weapon modifier (150%)
    /// </summary>
    [Serializable]
    public class Attribute
    {
        public event Action<Attribute> Changed;

        [SerializeField] private List<Modifier> _modifiers = new();

        public string ID { get; }
        public float BaseValue { get; private set; }
        [field: SerializeField] public float FinalValue { get; private set; }

        public Attribute(string id, float baseValue)
        {
            ID = id;
            BaseValue = baseValue;

            UpdateFinalValue();
        }
        
        public void SetValue(float value)
        {
            BaseValue = value;

            UpdateFinalValue();
        }

        public void AddModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);

            UpdateFinalValue();
        }

        public void RemoveModifier(string id)
        {
            var modifiers = _modifiers.Where(x => x.ID.Equals(id)).ToList();
            foreach (var modifier in modifiers)
            {
                _modifiers.Remove(modifier);
            }

            UpdateFinalValue();
        }

        private void UpdateFinalValue()
        {
            //Logger.Warning($"attribute: {ID}; value: {BaseValue}");

            var additionalValue = 0f;
            var multiplicationFactor = 0f;
            var percentage = 0f;

            foreach (var modifier in 
                     from modifier in _modifiers 
                     where modifier.Value != 0 
                     select modifier)
            {
                Logger.Log($"modifier: {modifier.ID}; value: {modifier.Value}");
                
                switch (modifier.Type)
                {
                    case EModificationType.Add:
                        additionalValue += modifier.Value;
                        break;
                    case EModificationType.Multiply:
                        multiplicationFactor += modifier.Value;
                        break;
                    case EModificationType.Percentage:
                        percentage += modifier.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            FinalValue = (BaseValue + additionalValue) * (1 + multiplicationFactor) * (1 + percentage / 100);

            if (Math.Abs(BaseValue - FinalValue) > .001f)
            {
                Logger.Warning($"attribute: {ID}; value: {BaseValue}; final: {FinalValue}");
            }
            
            Changed?.Invoke(this);
        }
    }
}