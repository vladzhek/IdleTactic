using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Configs;
using Slime.Configs.Rewards;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.Models.Abstract;
using Slime.UI.Footer.Dungeons;
using UnityEngine;

namespace Slime.Models
{
    public class DungeonsUIModel : BaseProgressModel, IDungeonsUIModel, IAuthorizedResourceUser
    {
        private readonly IResourcesModel _resourcesModel;
        private readonly IStageController _stageController;
        private readonly ITimeTrackerService _timeTrackerService;
        private readonly DailyLimitsSettings _dailyLimitsSettings;
        private readonly IStageModel _stageModel;
        private readonly IAdsService _adsService;

        // TODO: move to rewards
        public string AuthorizationToken => Data.Constants.System.RESOURCE_TOKEN;

        public DungeonsUIModel(IResourcesModel resourcesModel, IStageController stageController,
            ITimeTrackerService timeTrackerService, IStageModel stageModel, IGameProgressModel progressModel,
            IAdsService adsService) : base(progressModel)
        {
            _stageController = stageController;
            _resourcesModel = resourcesModel;
            _timeTrackerService = timeTrackerService;
            _stageModel = stageModel;
            _adsService = adsService;

            _dailyLimitsSettings = Resources.Load<DailyLimitsSettings>(DailyLimitsSettings.PATH);
        }

        protected override void OnProgressLoaded()
        {
            AddedKeysForNewDay();
        }

        public async void LoadDungeon(EStageType type)
        {
            if (_stageModel.Type == EStageType.Default)
            {
                if (GetAvailableKeysValue(type) > 0 || await IsLoadDungeonForAd(type))
                {
                    _stageModel.SetType(type);
                    _stageController.StageCompleted += OnStageCompleted;
                }
            }
        }

        private async Task<bool> IsLoadDungeonForAd(EStageType type)
        {
            if (IsDailyDungeonAdsAvailable(type)
                && _stageController.Type == EStageType.Default)
            {
                if (await _adsService.ShowAd(AdPlacementIDs.DUNGEON))
                {
                    GameData.AdsData[_dailyLimitsSettings.GetDungeonLimitsDictionary()[type].Resource]++;

                    _resourcesModel.TryAdd(this,
                        _dailyLimitsSettings.GetDungeonLimitsDictionary()
                            [type].Resource);

                    return true;
                }
            }

            return false;
        }

        public bool IsDailyDungeonAdsAvailable(EStageType type)
        {
            var adsViewedToday = GetAdsViewedTodayProgress(type);

            return adsViewedToday < _dailyLimitsSettings.GetDungeonLimitsDictionary()[type].DailyAdsLimit;
        }

        public int GetAvailableKeysValue(EStageType type)
        {
            // NOTE: ?
            const EResource boss = EResource.BossRushCurrency;
            const EResource gold = EResource.GoldRushCurrency;
            return type switch
            {
                EStageType.BossRush when _resourcesModel.IsEnough(boss) => (int)_resourcesModel.Get(boss),
                EStageType.GoldRush when _resourcesModel.IsEnough(gold) => (int)_resourcesModel.Get(gold),
                _ => 0
            };
        }

        public int GetAvailableAdsValue(EStageType type)
        {
            return _dailyLimitsSettings.GetDungeonLimitsDictionary()[type].DailyAdsLimit -
                   GetAdsViewedTodayProgress(type);
        }

        public int GetDungeonLevel(EStageType type)
        {
            // TODO: remove gamedata
            var data = GameData.StageData.GetValueOrDefault(type);
            return data?.Stage ?? 0;
        }

        private void OnStageCompleted(IStageController stage)
        {
            if (stage.Type == EStageType.Default)
            {
                return;
            }

            _stageController.StageCompleted -= OnStageCompleted;

            SpendDungeonKey(stage.Type);
        }

        private void SpendDungeonKey(EStageType type)
        {
            const EResource boss = EResource.BossRushCurrency;
            const EResource gold = EResource.GoldRushCurrency;
            switch (type)
            {
                case EStageType.BossRush when _resourcesModel.IsEnough(boss):
                    _resourcesModel.TrySpend(this, boss);
                    break;
                case EStageType.GoldRush when _resourcesModel.IsEnough(gold):
                    _resourcesModel.TrySpend(this, gold);
                    break;
                case EStageType.Default:
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, $"type \"{type}\" not allowed here");
            }
        }

        private int GetAdsViewedTodayProgress(EStageType type)
        {
            GameData.AdsData ??= new Dictionary<EResource, int>();
            var data = GameData.AdsData;
            var resourceId = _dailyLimitsSettings.GetDungeonLimitsDictionary()[type].Resource;

            // TODO: why UIModel looks in GameData?
            if (data.TryGetValue(resourceId, out var value))
            {
                return value;
            }

            data.Add(resourceId, 0);
            return GameData.AdsData[resourceId];
        }

        private void AddedKeysForNewDay()
        {
            if (_timeTrackerService.IsNewDay())
            {
                foreach (var dungeonKeysLimit in _dailyLimitsSettings.GetDungeonLimitsDictionary().Values)
                {
                    var haveKeys = GetAvailableKeysValue(dungeonKeysLimit.StageType);
                    var getKeys = dungeonKeysLimit.Quantity - haveKeys;
                    _resourcesModel.TryAdd(this, dungeonKeysLimit.Resource, getKeys);
                }
            }
        }
    }
}