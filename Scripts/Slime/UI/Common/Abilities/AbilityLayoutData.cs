using UI.Base.Widgets;
using UnityEngine;

namespace Slime.UI.Common.Abilities
{
    public class AbilityLayoutData : LayoutData<AbilityLayoutData>
    {
        public readonly bool IsUnlocked;
        
        public AbilityLayoutData(Sprite sprite, bool isUnlocked) : base(sprite)
        {
            IsUnlocked = isUnlocked;
        }
        
        protected override bool IsEqualTo(AbilityLayoutData other) =>
            Equals(Sprite, other.Sprite) && 
            IsUnlocked == other.IsUnlocked && 
            IsSelected == other.IsSelected;

        protected override int HashCode => System.HashCode.Combine(
            Sprite,
            IsUnlocked,
            IsSelected);

        public override string ToString()
        {
            return $"{base.ToString()} unlocked: {IsUnlocked}; selected: {IsSelected}";
        }
    }
}