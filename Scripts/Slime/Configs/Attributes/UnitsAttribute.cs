using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.Data.IDs;
using UnityEngine;

// TODO: remove
namespace Slime.Configs.Attributes
{
    [Serializable]
    public class UnitsAttribute
    {
        [SerializeField, ValueDropdown(nameof(IDs))]
        private string _unitId;

        [SerializeField] private AttributesUpgradesConfig _upgradesConfig;

        public IEnumerable<string> IDs => EnemyArchetypeIDs.Values;

        public string UnitId => _unitId;
        public AttributesUpgradesConfig UpgradesConfig => _upgradesConfig;
    }
}