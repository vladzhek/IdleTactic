using System.Collections.Generic;
using System.Linq;
using Data;
using Slime.AbstractLayer.Configs;
using Slime.Configs.Attributes;
using Slime.Configs.Rewards;
using Slime.Configs.Units;
using Slime.Data.Enums;
using Slime.Levels;
using UnityEngine;

namespace Slime.Configs.Levels
{
    [CreateAssetMenu(fileName = "Level", menuName = "Assets/Configs/Level", order = 0)]
    public class StageConfig : ScriptableObject, IStageConfig
    {
        [SerializeField] private EStageType _stageType;
        [SerializeField] private EnvironmentConfig _environmentData;
        [SerializeField] private UnitsAvatars _unitsAvatars;
        [SerializeField] private WaveData[] _waves;
        [SerializeField] private RewardsConfig _rewardsConfig;
        [SerializeField] private UnitsAttribute[] _unitsUpgradesConfig;
        [SerializeField] private bool _isTimer;
        [SerializeField] private int _timeDuration;


        public EStageType StageType => _stageType;
        public IEnvironmentConfig EnvironmentData => _environmentData;
        public IWaveConfig[] Waves => GetWaveConfig();
        public IRewardsConfig RewardsConfig => _rewardsConfig;

        public Dictionary<string, IAttributeUpgradesData[]> UpgradesConfig =>
            _unitsUpgradesConfig.ToDictionary(x => x.UnitId, x => x.UpgradesConfig.AttributeUpgradesData);

        public bool IsTimer => _isTimer;

        public int TimeDuration => _timeDuration;

        private IWaveConfig[] GetWaveConfig()
        {
            var result = new IWaveConfig[_waves.Length];

            for (var i = 0; i < _waves.Length; i++)
            {
                var waveData = _waves[i];
                result[i] = new WaveConfig(waveData.Units, _unitsAvatars);
            }

            return result;
        }
    }
}