using System;
using Slime.Data.Enums;
using UnityEngine;
using Utils;

// ReSharper disable InconsistentNaming

namespace Slime.Data.Abstract
{
    [Serializable]
    public abstract class EntityData : UpgradableData, IDisplayable, IEquipable
    {
        // TODO: replace int value with struct[] { attributeID, value }

        #region IDisplayable, IEquipable
        
        public string Title => _title;
        public virtual string Description => _description;
        public int Order => _order;
        public Sprite Sprite { get; set; }
        public ERarity Rarity => _rarity;
        public virtual float ActiveValue => Progression.Percentage(_baseActiveValue, ProgressionLevel, _activeValueStep);
        public virtual float PassiveValue => Progression.Percentage(_basePassiveValue, ProgressionLevel);
        public bool IsEquipped { get; set; }
        
        #endregion
        
        // private
        
        [Header("Entity")]
        [SerializeField] protected string _title;
        [SerializeField, TextArea] protected string _description;
        [SerializeField] protected int _order;
        [SerializeField] protected ERarity _rarity;
        [Space]
        [SerializeField] protected float _baseActiveValue;
        [SerializeField] protected float _activeValueStep = .1f;
        [SerializeField] protected float _basePassiveValue;
    }
}