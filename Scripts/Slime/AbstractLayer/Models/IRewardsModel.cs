using System.Collections.Generic;
using Slime.Data.Products;
using Utils.Promises;

namespace Slime.AbstractLayer.Models
{
    public interface IRewardsModel
    {
        // TODO: why promise and not unitask?
        // TODO: why different methods?
        public Promise AddRewardForUnit(IEnumerable<ResourceData> rewards);
        public Promise AddRewardForStage(IEnumerable<ResourceData> rewards);
    }
}