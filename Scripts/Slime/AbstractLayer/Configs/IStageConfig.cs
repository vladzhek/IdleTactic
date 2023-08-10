using System.Collections.Generic;
using Slime.Data;
using Slime.Data.Enums;
using Slime.Data.IDs;

namespace Slime.AbstractLayer.Configs
{
    public interface IStageConfig
    {
        EStageType StageType { get; }
        IEnvironmentConfig EnvironmentData { get; }
        IWaveConfig[] Waves { get; }
        IRewardsConfig RewardsConfig { get; }
        Dictionary<string, IAttributeUpgradesData[]> UpgradesConfig { get; }
        bool IsTimer { get; }
        int TimeDuration { get; }
    }
}