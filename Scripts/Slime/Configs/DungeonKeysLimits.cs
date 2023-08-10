using System;
using Slime.Data.Enums;
using UnityEngine;

namespace Slime.Configs
{
    [Serializable]
    public class DungeonKeysLimits
    {
        // NOTE: why we need stage type here
        [SerializeField] private EStageType _stageType;
        [SerializeField] private EResource _resource;
        [SerializeField] private int _quantity;
        [SerializeField] private int _dailyAdsLimit;
        
        public EStageType StageType => _stageType;
        public EResource Resource => _resource;
        public int Quantity => _quantity;
        public int DailyAdsLimit => _dailyAdsLimit;
    }
}