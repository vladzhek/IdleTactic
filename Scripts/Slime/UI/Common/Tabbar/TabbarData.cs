using Slime.UI.Abstract;
using UI.Base.Widgets;
using UnityEngine;

namespace Slime.UI.Common.Tabbar
{
    public class TabbarData : LayoutData<TabbarData>
    {
        // TODO: move to constructor
        public new string Title;
        public bool IsUnlocked;
        
        public TabbarData(Sprite sprite) : base(sprite)
        {
        }
        
        protected override bool IsEqualTo(TabbarData other) =>
            base.IsEqualTo(other)
            && Equals(Title, other.Title) 
            && IsUnlocked == other.IsUnlocked;

        protected override int HashCode => System.HashCode.Combine(
            base.HashCode,
            Title,
            IsUnlocked);

        public override string ToString()
        {
            return $"{base.ToString()}" 
                   + $" title: {Title};"
                   + $" sprite: {Sprite};"
                   + $" unlocked: {IsUnlocked};"
                   + $" open: {IsSelected}"
                   ;
        }
    }
}