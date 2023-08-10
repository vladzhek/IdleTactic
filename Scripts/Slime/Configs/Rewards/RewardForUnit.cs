using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using Slime.Data.IDs;
using Slime.Data.Products;
using UnityEngine;

namespace Slime.Configs.Rewards
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RewardForUnit
    {
        public string UnitId => _unitId;
        public IEnumerable<ResourceData> Rewards => _rewards;

        [SerializeField, ValueDropdown(nameof(IDs))] private string _unitId;
        [SerializeField] private ResourceData[] _rewards;

        private IEnumerable<string> IDs => EnemyArchetypeIDs.Values;
    }
}