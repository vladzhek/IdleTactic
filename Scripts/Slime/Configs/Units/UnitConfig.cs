using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Slime.Data.IDs;
using UnityEngine;

namespace Slime.Configs
{
    [Serializable]
    public struct UnitConfig
    {
        [SerializeField, ValueDropdown(nameof(UnitIDs))] private string _unitID;
        [SerializeField] private UnitDefaultParameters _unitBaseAttributesValues;

        public IEnumerable<string> UnitIDs => CharacterIDs.Values.Concat(EnemyArchetypeIDs.Values).
            Concat(CompanionIDs.Values);

        public string UnitID => _unitID;
        public UnitDefaultParameters BaseAttributesValues => _unitBaseAttributesValues;

        public UnitConfig(string unitID)
        {
            _unitID = unitID;
            _unitBaseAttributesValues = null;
        }
    }
}