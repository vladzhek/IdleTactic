using System;
using System.Linq;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Configs.Offline;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.Models.Abstract;
using UnityEngine;
using Utils;

namespace Slime.Models
{
    public class OfflineIncomeModel : BaseProgressModel, 
        IAuthorizedResourceUser, 
        IOfflineIncomeModel
    {
        public event Action<int> OnOfflineIncome;

        private readonly IStageController _stageController;
        private readonly OfflineIncomeConfig _incomeConfig;
        private readonly IUnitsModel _unitsModel;
        private readonly IPlayer _player;
        private readonly IAdsService _adsService;
        private readonly IResourcesModel _resourcesModel;

        // TODO: move to rewards
        public string AuthorizationToken => Data.Constants.System.RESOURCE_TOKEN;

        private bool _isFirstPlay = true;

        public int OfflineIncomeValue { get; private set; }

        public OfflineIncomeModel(
            IGameProgressModel progressModel, 
            IStageController stageController,
            IUnitsModel unitsModel, 
            IPlayer player, 
            IResourcesModel resourcesModel, 
            IAdsService adsService) : base(progressModel)
        {
            _incomeConfig = Resources.Load<OfflineIncomeConfig>(OfflineIncomeConfig.PATH);
            _stageController = stageController;
            _unitsModel = unitsModel;
            _player = player;
            _resourcesModel = resourcesModel;
            _adsService = adsService;
        }

        public override void Initialize()
        {
            _stageController.StageStarted += OnStageStarted;
        }

        public override void Dispose()
        {
            _stageController.StageStarted -= OnStageStarted;
        }
        
        private void OnStageStarted(IStageController stage)
        {
            if (_isFirstPlay)
            {
                OfflineIncome();

                _isFirstPlay = false;
            }
        }

        private void OfflineIncome()
        {
            var howManyHoursHavePassed = HowManyHoursHavePassed();
            var incomeModifier = 0f;

            foreach (var data in _incomeConfig.Items)
            {
                if (data.Hours > howManyHoursHavePassed)
                {
                    incomeModifier = 1 - data.RewardCoefficient;
                    break;
                }
            }

            var middleHealthForAllUnits = MiddleHealthForAllUnits();
            var middleRewardForAllUnits = AverageReward();
            // TODO: this values are not ready, need to wait for ParametersModel.OnDPSChange
            var playerAttack = _player.Unit.GetAttribute(AttributeIDs.Damage)?.FinalValue ?? 10;
            var playerAttackSpeed = _player.Unit.GetAttribute(AttributeIDs.AttackSpeed)?.FinalValue ?? 1;
            var howManyAttackToKill = middleHealthForAllUnits / playerAttack;
            var timeToKill = howManyAttackToKill * playerAttackSpeed;

            var killsForHavePassed = FormatTime.HoursIntFormat(howManyHoursHavePassed) / timeToKill;

            OfflineIncomeValue = (int)(killsForHavePassed * middleRewardForAllUnits * incomeModifier);

            OnOfflineIncome?.Invoke(OfflineIncomeValue);
        }

        private int HowManyHoursHavePassed()
        {
            return (int)(DateTime.Now - GameData.DateOfLastSession).TotalHours;
        }

        private float MiddleHealthForAllUnits()
        {
            // NOTE: only get enemy units
            var health = _unitsModel.Units.Collection
                .Sum(unit => unit.GetAttribute(AttributeIDs.Health)?.BaseValue ?? 0);

            return health / _unitsModel.Units.Collection.Count;
        }

        private float AverageReward()
        {
            var rewards = _stageController.CurrentStage.StageConfig.RewardsConfig.RewardForUnits.Values;
            var rewardSum = rewards.Sum(reward => reward.ElementAt(0).Quantity);

            return rewardSum / rewards.Count;
        }

        public void AddReward(int value)
        {
            _resourcesModel.TryAdd(this, EResource.SoftCurrency, value);
        }

        public async void ShowAd()
        {
            if (await _adsService.ShowAd(AdPlacementIDs.OFFLINE_INCOME))
            {
                AddReward(OfflineIncomeValue);
            }
        }
    }
}