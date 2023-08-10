using System;
using Slime.Data.Skills;
using UnityEngine;

namespace Slime.Data.Inventory
{
    public class CompanionPanelViewData : IEquatable<CompanionPanelViewData>
    {
        public string ID;
        public Sprite Sprite;
        public float Cooldown;
        public bool IsOpened;
        public bool IsEquipped;
        public bool IsCanEquipped;

        public bool Equals(CompanionPanelViewData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ID == other.ID && Cooldown.Equals(other.Cooldown);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CompanionPanelViewData)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Cooldown);
        }
    }
}