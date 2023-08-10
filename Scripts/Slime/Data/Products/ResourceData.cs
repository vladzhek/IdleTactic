using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using Slime.Data.Enums;
using Slime.Data.IDs;
using UnityEngine;

namespace Slime.Data.Products
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ResourceData
    {
        public EResource Resource => _resource;
        public float Quantity => _quantity;
        public string EntityID => _entityID; // e.g: EResource.Character, entityID = CharacterIDs.TANK_1

        public ResourceData(EResource resource, float quantity)
        {
            _resource = resource;
            _quantity = quantity;
        }

        public static ResourceData operator ++(ResourceData resource)
        {
            resource._quantity++;
            return resource;
        }
        
        public static ResourceData operator +(ResourceData resource, float quantity)
        {
            resource._quantity += quantity;
            return resource;
        }
        
        public static ResourceData operator --(ResourceData resource)
        {
            resource._quantity--;
            return resource;
        }
        
        public static ResourceData operator -(ResourceData resource, float quantity)
        {
            resource._quantity -= quantity;
            return resource;
        }
        
        [Header("Resource")]
        [SerializeField] private EResource _resource;
        [SerializeField] private float _quantity;
        [SerializeField, ValueDropdown(nameof(IDs)), ShowIf("_resource", EResource.Character)] 
        private string _entityID;

        private IEnumerable<string> IDs => CharacterIDs.Values;
    }
}