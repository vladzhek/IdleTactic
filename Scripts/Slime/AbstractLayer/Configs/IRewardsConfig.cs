using System.Collections.Generic;
using Slime.Data.Products;

namespace Slime.AbstractLayer.Configs
{
    public interface IRewardsConfig
    {
        IEnumerable<ResourceData> RewardForWave { get; }
        Dictionary<string, IEnumerable<ResourceData>> RewardForUnits { get; }
    }
}