using System;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using Slime.Data.Progress.Abstract;
using UI.Base.Widgets;
using UnityEngine;
using Utils.Extensions;
using Logger = Utils.Logger;

namespace Slime.UI.Common.Equipment
{
    public class GridLayoutData : LayoutData<GridLayoutData>, ILayoutElementData
    {
        public GridLayoutData(ISummonable data, bool needsAttention = false) :
            base(data.Title, data.Sprite)
        {
            NeedsAttention = needsAttention;
            data.CopyPropertiesTo(this);
        }

        #region ILayoutElementData implementation
        
        public bool NeedsAttention { get; }
        
        public string ID { get; private set; }
        public string SpriteID => throw new NotImplementedException("irrelevant");
        public new Sprite Sprite { get; set; }
        public string Description { get; private set; }
        public int Order { get; private set; }
        public ERarity Rarity { get; private set; }
        public float ActiveValue { get; private set; }
        public float PassiveValue { get; private set; }
        public object Clone()
        {
            throw new NotImplementedException("don't upgrade through layout");
        }

        public bool IsEquipped { get; set; }
        public bool IsUnlocked { get; private set; }

        public int Level { get; private set; }
        public float Quantity { get; private set; }
        public int UpgradeQuantity { get; private set; }
        public float Progress { get; private set; }
        public bool CanUpgrade { get; private set; }

        public void SetProgress(ProgressData data)
        {
            if (data.Level != null)
            {
                Level = data.Level.Value;
            }
            
            if (data.Quantity != null)
            {
                Quantity = data.Quantity.Value;
            }
        }

        public void Upgrade()
        {
            Logger.Warning($"don't upgrade through layout, only through associated model with access to ProgressData");
            throw new NotImplementedException();
        }
        
        public void UpgradeOne()
        {
            Logger.Warning($"don't upgrade through layout, only through associated model with access to ProgressData");
            throw new NotImplementedException();
        }

        #endregion
        
        #region IEquatable implementation

        private bool IsEqualTo(ILayoutElementData other) =>
            Equals(ID, other.ID)
            && NeedsAttention == other.NeedsAttention
            //&& Equals(Title, other.Title)
            //&& Equals(Sprite, other.Sprite)
            && Rarity == other.Rarity
            //&& ActiveValue == other.ActiveValue
            //&& PassiveValue == other.PassiveValue
            && IsEquipped == other.IsEquipped
            && IsUnlocked == other.IsUnlocked
            && Level == other.Level
            && Math.Abs(Quantity - other.Quantity) < 0.01
            && UpgradeQuantity == other.UpgradeQuantity
            && Math.Abs(Progress - other.Progress) < .05;

        protected override int HashCode {
            get {
                var hashCode = new HashCode();
                hashCode.Add(ID);
                hashCode.Add(NeedsAttention);
                //hashCode.Add(Title);
                //hashCode.Add(Sprite);
                hashCode.Add(Rarity);
                //hashCode.Add(ActiveValue);
                //hashCode.Add(PassiveValue);
                hashCode.Add(IsEquipped);
                hashCode.Add(IsUnlocked);
                hashCode.Add(Level);
                hashCode.Add(Quantity);
                hashCode.Add(UpgradeQuantity);
                hashCode.Add(Progress);
                hashCode.Add(IsSelected);
                return hashCode.ToHashCode();
            }
        }

        public bool Equals(ILayoutElementData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsEqualTo(other);
        }
        
        #endregion
    }
}