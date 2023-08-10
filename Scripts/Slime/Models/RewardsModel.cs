using System.Collections.Generic;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using Slime.Data.Products;
using Slime.Services;
using Utils.Promises;

namespace Slime.Models
{
    // TODO: refactor this?
    [UsedImplicitly]
    public class RewardsModel : IRewardsModel, IAuthorizedResourceUser
    {
        private const float REWARD_FACTOR_BY_LEVEL = 2f;
        public string AuthorizationToken => Data.Constants.System.RESOURCE_TOKEN;
        
        public float RewardFactorForBooster { get; set; } = 1;

        private readonly IStageModel _stageModel;
        private readonly IResourcesModel _resourcesModel;
        
        public RewardsModel(
            IStageModel stageModel,
            IResourcesModel resourcesModel)
        {
            _stageModel = stageModel;
            _resourcesModel = resourcesModel;
        }

        // returns current normal progress
        private int CurrentStage => _stageModel?.Get(EStageType.Default)?.Stage ?? 0;

        public Promise AddRewardForUnit(IEnumerable<ResourceData> rewards)
        {
            return new Promise(() => { AddRewards(rewards); });
        }

        public Promise AddRewardForStage(IEnumerable<ResourceData> rewards)
        {
            return new Promise(() => { AddRewards(rewards); });
        }

        private void AddRewards(IEnumerable<ResourceData> rewards)
        {
            foreach (var reward in rewards)
            {
                var resource = reward.Resource;
                var value = reward.Quantity;
                
                // should be standard upgrades
                

                _resourcesModel.TryAdd(this, resource, value);
            }
        }
    }
}