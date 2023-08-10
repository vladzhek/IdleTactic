using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer.Configs;
using Slime.Data.Enums;
using Slime.Data.Products;
using UnityEngine;

namespace Slime.Configs.Rewards
{
    // TODO: refactor
    [CreateAssetMenu(fileName = "RewardsConfig", menuName = "Assets/Configs/Rewards", order = 0)]
    public class RewardsConfig : ScriptableObject, IRewardsConfig
    {
        [SerializeField] private EStageType _stageType;
        [SerializeField] private RewardForUnit[] _rewardForUnits;
        [SerializeField] private ResourceData[] _rewardForWave;

        private Dictionary<string, IEnumerable<ResourceData>> _rewards;

        public EStageType StageType => _stageType;
        public IEnumerable<ResourceData> RewardForWave => _rewardForWave;

        public Dictionary<string, IEnumerable<ResourceData>> RewardForUnits
        {
            get { return _rewards ?? _rewardForUnits.ToDictionary(x => x.UnitId, x => x.Rewards); }
        }
    }
}