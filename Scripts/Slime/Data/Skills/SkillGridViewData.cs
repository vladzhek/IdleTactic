using System;
using Slime.Data.Enums;
using UnityEngine;

namespace Slime.Data.Skills
{
    // TODO: replace with GridLayoutElement
    public class SkillGridViewData : IEquatable<SkillGridViewData>
    {
        public ESkillType ID;
        public int Value;
        public int NextUpgradeValue;
        public int Level;
        public Sprite Sprite;
        public bool IsAvailable;
        public bool IsEquipped;

        public bool Equals(SkillGridViewData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ID == other.ID && Value == other.Value && NextUpgradeValue == other.NextUpgradeValue &&
                   Equals(Sprite, other.Sprite) && IsAvailable == other.IsAvailable;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SkillGridViewData)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)ID, Value, NextUpgradeValue, Sprite, IsAvailable);
        }
    }
}