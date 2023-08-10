using System;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using UnityEngine;
using Utils;

// ReSharper disable InconsistentNaming

namespace Slime.Data.Attributes
{
    [Serializable]
    public class AttributeData : EntityData
    {
        public EAttribute Type => _type;
        public string SpriteId => $"attribute{Type}";

        // EntityData overrides
        
        public override string ID => $"{Type}";
        public override bool CanUpgrade => IsEnoughResources;
        
        public override void Upgrade()
        {
            if (!CanUpgrade)
            {
                throw new Exception($"can't be upgraded (isUnlocked: {IsUnlocked}; progress: {Progress})");
            }

            Level++;
        }
        
        public override string ToString()
        {
            return $"{base.ToString()} id: {ID}";
        }
        
        public float UpgradeCost => Progression.Percentage(_baseCostValue, ProgressionLevel);
        public bool IsEnoughResources { get; set; }
        public bool IsUpgradable => _isUpgradable;

        [Header("Attribute")]
        [SerializeField] private bool _isUpgradable;
        [SerializeField] private EAttribute _type;
        [SerializeField] private float _baseCostValue;
    }
}