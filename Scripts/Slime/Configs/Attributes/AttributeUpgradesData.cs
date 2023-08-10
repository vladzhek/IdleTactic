using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.AbstractLayer.Configs;
using Slime.Data.IDs;
using UnityEngine;

// TODO: remove
namespace Slime.Configs.Attributes
{
    [Serializable]
    public class AttributeUpgradesData : IAttributeUpgradesData
    {
        [SerializeField, ValueDropdown(nameof(IDs))]
        private string _attributeID;

        [SerializeField] private UpgradeConfig _upgradesConfig;

        public IEnumerable<string> IDs => AttributeIDs.Values;

        public string AttributeID => _attributeID;
        public IUpgradeConfig Config => _upgradesConfig;
    }
}