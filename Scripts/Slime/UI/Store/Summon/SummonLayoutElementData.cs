using System;
using Slime.Data.Enums;

namespace Slime.UI.Store.Summon
{
    public class SummonLayoutElementData
    {
        public SummonLayoutElementData(
            ESummonType type,
            bool canSummonLow,
            bool canSummonHigh,
            string lowSummonTitle, 
            string highSummonTitle,
            int lowSummonQuantity,
            int highSummonQuantity,
            int adSummonQuantity,
            int adMaxSummonQuantity,
            int adRemainingViewsQuantity,
            int adMaxViewsQuantity,
            int level,
            int quantity,
            int upgradeQuantity)
        {
            Type = type;
            CanSummonLow = canSummonLow;
            CanSummonHigh = canSummonHigh;
            LowSummonTitle = lowSummonTitle;
            HighSummonTitle = highSummonTitle;
            LowSummonQuantity = lowSummonQuantity;
            HighSummonQuantity = highSummonQuantity;
            AdSummonQuantity = adSummonQuantity;
            AdMaxSummonQuantity = adMaxSummonQuantity;
            AdRemainingViewsQuantity = adRemainingViewsQuantity;
            AdMaxViewsQuantity = adMaxViewsQuantity;
            Level = level;
            Quantity = quantity;
            UpgradeQuantity = upgradeQuantity;
        }

        public ESummonType Type { get; }
        public bool CanSummonLow { get; }
        public bool CanSummonHigh { get; }
        public string LowSummonTitle { get; }
        public string HighSummonTitle { get; }
        public int LowSummonQuantity { get; }
        public int HighSummonQuantity { get; }
        public int AdSummonQuantity { get; }
        public int AdMaxSummonQuantity { get; }
        public int AdRemainingViewsQuantity { get; }
        public int AdMaxViewsQuantity { get; }
        public int Level { get; }
        public int Quantity { get; }
        public int UpgradeQuantity { get; }
        public float Progress => UpgradeQuantity == 0 ? 0 : (float)Quantity / UpgradeQuantity;
        public bool CanUpgrade => Progress >= 1;
        
        public void Upgrade() => throw new NotImplementedException("upgrade through model");

        public override string ToString()
        {
            return $"{base.ToString()}" 
                   + $" type: {Type};"
                   + $" level: {Level};"
                   + $" quantity: {Quantity};"
                   + $" upgrade: {UpgradeQuantity}"
                   + $" progress: {Progress}"
                   ;
        }
    }
}