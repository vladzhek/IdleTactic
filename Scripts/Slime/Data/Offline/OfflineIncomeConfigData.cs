using System;
using UnityEngine;

namespace Slime.Data.Offline
{
    [Serializable]
    public class OfflineIncomeData
    {
        public int Hours => _hours;
        public float RewardCoefficient => _rewardCoefficient;
        
        // private
        [Header("Offline income")] 
        [SerializeField] private int _hours;
        [SerializeField, Range(0, 1)] private float _rewardCoefficient;
    }
}