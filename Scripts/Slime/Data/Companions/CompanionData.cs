using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.Data.Abstract;
using Slime.Data.IDs;
using UnityEngine;

namespace Slime.Data.Inventory
{
    [Serializable]
    public class CompanionData : EntityData, ISummonable
    {
        public override string ID { 
            get => _companionID;
            set => _companionID = value;
        }
        
        [Header("Companion")]
        [SerializeField] [ValueDropdown(nameof(IDs))] private string _companionID;
        private IEnumerable<string> IDs => CompanionIDs.Values;
    }
}