using UI.Base.Widgets;
using UnityEngine;

namespace Slime.UI.Rewards
{
    public class RewardsLayoutData : LayoutData<RewardsLayoutData>
    {
        public readonly string Quantity;

        public RewardsLayoutData(Sprite sprite, string quantity) : base(sprite)
        {
            Quantity = quantity;
        }

        protected override bool IsEqualTo(RewardsLayoutData other) =>
            base.IsEqualTo(other) 
            && Quantity == other.Quantity;

        protected override int HashCode => System.HashCode.Combine(
            base.HashCode, 
            Quantity
            );
    }
}