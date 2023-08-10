using System;
using Slime.Data.Enums;
using UI.Base.Widgets;

namespace Slime.UI.Popups.Summon
{
    public class SummonInfoData : LayoutData<SummonInfoData>
    {
        public readonly ERarity Rarity;
        public readonly float Percentage;
        
        public SummonInfoData(ERarity rarity, float percentage) : base(rarity.ToString())
        {
            Rarity = rarity;
            Percentage = percentage;
        }
        
        protected override bool IsEqualTo(SummonInfoData other) =>
            Equals(Title, other.Title) 
            && Equals(Rarity, other.Rarity)
            && Math.Abs(Percentage - other.Percentage) < 0.01;

        protected override int HashCode => System.HashCode.Combine(
            Title, 
            Rarity,
            Percentage);
    }
}