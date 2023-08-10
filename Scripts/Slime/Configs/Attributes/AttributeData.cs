using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// TODO: remove
namespace Slime.Configs.Attributes
{
    [Serializable]
    public struct AttributeData
    {
        [SerializeField, ValueDropdown(nameof(AttributesIDs))] private string _attributeID;
        [SerializeField] private float _value;

        public IEnumerable<string> AttributesIDs => Data.IDs.AttributeIDs.Values;

        public string AttributesID => _attributeID;
        public float Value => _value;
    }
}