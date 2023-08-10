using System;
using System.Collections.Generic;
using System.Linq;
using Slime.Data.IDs;
using Sirenix.OdinInspector;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Configs;
using Slime.UnityLayer;
using UnityEngine;

namespace Data.Units
{
    [Serializable]
    public struct UnitInfo : IUnitInfo
    {
        [SerializeField, ValueDropdown(nameof(UnitsID))]
        private string _id;

        [SerializeField, AssetSelector(Paths = "Assets/Prefabs/Units")]
        private UnitAvatar _unitAvatar;

        public string ID => _id;
        public IUnitAvatar UnitAvatar => _unitAvatar;

        private IEnumerable<string> UnitsID => CharacterIDs.Values.Concat(EnemyArchetypeIDs.Values).Concat(CompanionIDs.Values);

        public UnitInfo(string id)
        {
            _id = id;
            _unitAvatar = null;
        }
    }
}