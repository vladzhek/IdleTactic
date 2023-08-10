using System;
using UnityEngine;

namespace Slime.Data.Skills
{
    public class SkillPanelViewData : IEquatable<SkillPanelViewData>
    {
        public string ID;
        public Sprite Sprite;
        public float Cooldown;
        public bool IsOpened;
        public bool IsEquipped;
        public bool IsCanEquipped;

        public bool Equals(SkillPanelViewData other)
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
            return Equals((SkillPanelViewData)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Cooldown);
        }
    }
}