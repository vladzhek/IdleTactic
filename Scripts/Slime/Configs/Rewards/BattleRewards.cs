using Slime.Levels;
using UnityEngine;

namespace Slime.Configs.Rewards
{
    [CreateAssetMenu(fileName = "LevelRewards", menuName = "Assets/Configs/LevelRewards", order = 0)]
    public class BattleRewards : ScriptableObject
    {
        [SerializeField] private RewardsConfig[] _rewardConfig;

        public RewardsConfig[] Config => _rewardConfig;
    }
}