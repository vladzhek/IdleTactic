using UI.Base.Widgets;
using UnityEngine;

namespace Slime.UI.Attributes
{
    public class AttributeData : 
        LayoutData<AttributeData>
    {
        // NOTE: make this initialize from constructor
        public string ID;
        public new string Title;
        public string Level;
        public string Value;
        public string Cost;
        public bool CanUpgrade;
        
        public AttributeData(Sprite sprite) : base(sprite)
        {
        }
        
        #region BaseLayoutElementData overrides
        
        protected override bool IsEqualTo(AttributeData other) =>
            Title == other.Title
            && Level == other.Level
            && Value == other.Value
            && Cost == other.Cost
            && CanUpgrade == other.CanUpgrade;

        protected override int HashCode => System.HashCode.Combine(Title, Level, Value, Cost, CanUpgrade);

        #endregion
    }
}