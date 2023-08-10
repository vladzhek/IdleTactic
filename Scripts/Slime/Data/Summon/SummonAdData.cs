using System;
using Slime.Data.Progress.Abstract;
using UnityEngine;
using Utils;

namespace Slime.Data.Summon
{
    [Serializable]
    public class SummonAdData : SummonBaseData
    {
        // public
        
        public override string ID => $"{base.ID}Ad";
        
        public int SummonQuantity => (int)Progression.Linear(_initialSummonQuantity, Level);
        public int ViewsQuantity => _viewsQuantity;
        public int MaxViewsQuantity => _maxViewsQuantity;
        public int MaxSummonQuantity => _maxSummonQuantity;
        public int RemainingViewsQuantity => MaxViewsQuantity - ViewsQuantity;

        public override int UpgradeQuantity => 1;

        // for ads we store in views in ProgressData.Quantity
        public override void SetProgress(ProgressData data)
        {
            if (data?.Level != null) Level = data.Level.Value;
            if (data?.Quantity != null) _viewsQuantity = (int)data.Quantity.Value;
        }

        // SummonAdData.Quantity is only set for upgrades
        public override void Upgrade()
        {
            Quantity = 1;
            base.Upgrade();
        }

        public override string ToString()
        { 
            return $"{base.ToString()}"
                   + $" views quantity: {RemainingViewsQuantity};"
                ;
        }

        // private
        
        [Header("Ad")]
        [SerializeField] private int _initialSummonQuantity;
        [SerializeField] private int _maxSummonQuantity;
        [SerializeField] private int _maxViewsQuantity;
        private int _viewsQuantity;
    }
}