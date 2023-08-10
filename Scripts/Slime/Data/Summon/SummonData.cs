using System;
using System.Linq;
using Slime.Data.Enums;
using UnityEngine;
using Utils;

namespace Slime.Data.Summon
{
    [Serializable]
    public class SummonData : SummonBaseData
    {
        // public
        
        public virtual int LowSummonQuantity => _lowSummonQuantity;
        public virtual int HighSummonQuantity => _highSummonQuantity;
        public float LowSummonPrice => _lowSummonPrice;
        public float HighSummonPrice => _highSummonPrice;

        public virtual float GetProbability(ERarity rarity)
        {
            if (!ERarityExtensions.IsImplemented(rarity))
            {
                return 0;
            }

            // ensure that this rarity has base probability
            var baseProbability = _baseProbabilities.FirstOrDefault(p => p.Rarity == rarity);
            if (baseProbability == null)
            {
                return 0;
            }
            
            // get sum of all probabilities adjusted for level
            var sum = (from p in _baseProbabilities
                select Progression.Linear(p.Value, Level)).Sum();
            
            // normalize final probability
            return Progression.Linear(baseProbability.Value, Level) / sum;
        }
        
        // private

        [Header("Summon")]
        [SerializeField] private int _lowSummonQuantity;
        [SerializeField] private int _highSummonQuantity;
        [SerializeField] private int _lowSummonPrice;
        [SerializeField] private int _highSummonPrice;
        
        [Space]
        [SerializeField] private SummonProbabilityData[] _baseProbabilities = {};
    }

    [Serializable]
    internal class SummonProbabilityData
    {
        [SerializeField] private ERarity _rarity;
        [SerializeField] private float _value;

        public ERarity Rarity => _rarity;
        public float Value => _value;
    }
}