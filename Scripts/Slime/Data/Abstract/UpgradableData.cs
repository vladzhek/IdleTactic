using System;
using Slime.Data.Progress.Abstract;
using UnityEngine;
using Utils;

namespace Slime.Data.Abstract
{
    [Serializable]
    public abstract class UpgradableData : GenericData, IUpgradable
    {
        private const int MIN_LEVEL = 1;
        
        // public
        
        public bool IsUnlocked => _isUnlocked || _level > 0;
        public virtual int Level { 
            get => _isUnlocked ? Math.Max(_level, MIN_LEVEL) : _level;
            protected set => _level = value;
        }
        public virtual int ProgressionLevel => Mathf.Max(Level, MIN_LEVEL) - 1;
        public virtual float Quantity { get; protected set; }
        public virtual int UpgradeQuantity => (int)Math.Ceiling(Progression.Linear(1, ProgressionLevel));
        public virtual float Progress => UpgradeQuantity == 0 ? 0 : Quantity / UpgradeQuantity;
        public virtual bool CanUpgrade => IsUnlocked && Progress >= 1;
        
        public virtual void SetProgress(ProgressData data)
        {
            if (data?.Level != null) Level = data.Level.Value;
            if (data?.Quantity != null) Quantity = data.Quantity.Value;
        }
        
        public virtual void SetResourceQuantity(float quantity)
        {
            Quantity = quantity;
        }

        public virtual void Upgrade()
        {
            if (!CanUpgrade)
            {
                throw new Exception($"can't be upgraded (isUnlocked: {IsUnlocked}; progress: {Progress})");
            }

            while (CanUpgrade)
            {
                Quantity -= UpgradeQuantity;
                Level++;
            }
        }
        
        public virtual void UpgradeOne()
        {
            if (!CanUpgrade)
            {
                throw new Exception($"can't be upgraded (isUnlocked: {IsUnlocked}; progress: {Progress})");
            }

            Quantity -= UpgradeQuantity;
            Level++;
        }
        
        public override string ToString()
        { 
            return $"{base.ToString()};"
                   + $" level: {Level};"
                   + $" quantity: {Quantity};"
                   + $" upgrade quantity: {UpgradeQuantity};"
                   + $" progress: {Progress}"
                   + $" can upgrade: {CanUpgrade}"
                ;
        }
        
        // private
        
        [SerializeField] private bool _isUnlocked;
        private int _level;
    }
}