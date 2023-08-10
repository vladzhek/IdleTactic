using System;
using UnityEngine;

namespace Slime.UI.Boosters
{
    public class BoosterLayoutElementViewData : IEquatable<BoosterLayoutElementViewData>
    {
        public Sprite BoosterSprite;
        public Color ColorBG;
        public string ID;
        public string Title;
        public string Description;
        public float Value;
        public int CurrentLvl;
        public int Amount;
        public int TotalCounts;
        public int Duration;
        public bool IsActive;

        public BoosterLayoutElementViewData(string id)
        {
            ID = id;
        }

        public bool Equals(BoosterLayoutElementViewData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(BoosterSprite, other.BoosterSprite) && ColorBG.Equals(other.ColorBG) && ID == other.ID && Title == other.Title && Description == other.Description && Value.Equals(other.Value) && CurrentLvl == other.CurrentLvl && Amount == other.Amount && TotalCounts == other.TotalCounts && Duration == other.Duration && IsActive == other.IsActive;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BoosterLayoutElementViewData)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(BoosterSprite);
            hashCode.Add(ColorBG);
            hashCode.Add(ID);
            hashCode.Add(Title);
            hashCode.Add(Description);
            hashCode.Add(Value);
            hashCode.Add(CurrentLvl);
            hashCode.Add(Amount);
            hashCode.Add(TotalCounts);
            hashCode.Add(Duration);
            hashCode.Add(IsActive);
            return hashCode.ToHashCode();
        }
    }
}