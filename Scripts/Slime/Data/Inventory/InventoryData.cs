using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using Slime.Data.IDs;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Slime.Data.Inventory
{
    [Serializable]
    public class InventoryData : EntityData, ISummonable
    {
        // public
        
        public override string ID { 
            get => _inventoryID;
            set => _inventoryID = value;
        }

        public EInventoryType Type => _type;

        public override string ToString()
        {
            return $"{base.ToString()} id: {ID}";
        }
        
        // private
        
        [Header("Inventory")]
        [SerializeField, ValueDropdown(nameof(IDs))] private string _inventoryID;
        [SerializeField] private EInventoryType _type;

        private IEnumerable<string> IDs => InventoryIDs.Values;
    }
}