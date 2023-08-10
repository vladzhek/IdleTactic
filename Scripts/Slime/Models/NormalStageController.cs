using System;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using Slime.AbstractLayer.Configs;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.Data.IDs.Boosters;
using Slime.Data.Products;
using Slime.Data.Progress;
using Slime.Services.Analytics;
using Utils;
using Utils.Time;

// TODO: merge with StageModel?

namespace Slime.Models
{
    [UsedImplicitly]
    public class NormalStageController : IStageController
    {
        private const int REWARD_FACTOR_BY_LEVEL = 2;

        // dependencies
        private readonly IStageModel _stageModel;
        private readonly IStageFactory _stageFactory;
        private readonly TimerService _timerService;
        private readonly IAnalyticsManager _analyticsManager;
        private readonly IBoostersModel _boostersModel;

        private Timer _timer;

        // state
        private IDictionary<EStageType, IStageConfig[]> _stageConfigs;

        public NormalStageController(IStageModel stageModel, IStageFactory stageFactory, TimerService timerService,
            IAnalyticsManager analyticsManager, IBoostersModel boostersModel)
        {
            _stageModel = stageModel;
            _stageFactory = stageFactory;
            _timerService = timerService;
            _analyticsManager = analyticsManager;
            _boostersModel = boostersModel;
        }

        #region IStageController implementation

        public event Action<IStageController> WaveCompleted;
        public event Action<IStageController> StageChanged;
        public event Action<IStageController> StageCompleted;
        public event Action<IStageController> StageStarted;
        public event Action<IStageController> StageFailed;
        public IStage CurrentStage { get; private set; }
        public EStageType Type => _stageModel.Type;

        public void SetConfigs(IDictionary<EStageType, IStageConfig[]> configs)
        {
            _stageConfigs = configs;
        }

        public void LoadStageFromScratch()
        {
            var data = Progress(Type);
            Logger.Log($"data: {data}");

            var config = GetCurrentStageConfig(Type);

            if (config.IsTimer)
            {
                _timer = _timerService.CreateTimer(config.StageType.ToString(), config.TimeDuration);
                _timer.OnComplete += OnTimerCompleted;
            }

            var newStage = _stageFactory.Load(config, data);
            SetCurrentStage(newStage);
            var wave = data.Wave ?? 0;
            CurrentStage.LoadWave(data.Wave ?? 0);

            var stage = _stageModel.Get(Type);
            stage.Count++;

            _analyticsManager.Send(EEventType.StageStart, GetStageEventParameters());
            StageStarted?.Invoke(this);
        }

        public void LoadNextLevel()
        {
            Logger.Log();

            if (CurrentStage == null)
            {
                Logger.Error($"current stage null");
                return;
            }

            if (IsCycled)
            {
                var data = Progress(Type);
                if (CurrentStage.StageData.Stage == data.Stage && CurrentStage.StageData.Wave == data.Wave)
                {
                    CurrentStage.ReloadWave();
                }
                else
                {
                    LoadStageFromScratch();
                }

                return;
            }

            if (!CurrentStage.IsLastWave)
            {
                CurrentStage.LoadNextWave();
                UpdateProgress();
                return;
            }

            LoadStageFromScratch();
        }

        #endregion

        private StageData Progress(EStageType type)
        {
            return _stageModel.Get(type);
        }

        private bool IsCycled => _stageModel.Get(EStageType.Default)?.IsCycled ?? false;

        private IStageConfig GetCurrentStageConfig(EStageType type)
        {
            var data = Progress(type);
            var index = (data.Stage ?? 0) % _stageConfigs[type].Length;

            return _stageConfigs[type][index];
        }

        private IStageConfig GetNextLevelConfig()
        {
            var data = Progress(Type);
            var index = ((data.Stage ?? 0) + 1) % _stageConfigs[Type].Length;

            return _stageConfigs[Type][index];
        }

        private void SetCurrentStage(IStage stage)
        {
            if (CurrentStage != null)
            {
                CurrentStage.Completed -= OnStageCompleted;
                CurrentStage.WaveCompleted -= OnWaveCompleted;
                CurrentStage.WaveFailed -= OnWaveFailed;
                CurrentStage.WaveStarted -= OnWaveStarted;
                CurrentStage.Dispose();
            }

            CurrentStage = stage;
            UpdateProgress();

            CurrentStage.Completed += OnStageCompleted;
            CurrentStage.WaveCompleted += OnWaveCompleted;
            CurrentStage.WaveFailed += OnWaveFailed;
            CurrentStage.WaveStarted += OnWaveStarted;
        }

        private void UpdateProgress()
        {
            var stage = CurrentStage.StageData.Stage;
            var wave = CurrentStage.StageData.Wave;
            Logger.Log($"stage: {stage} wave: {wave}");
            _stageModel.Update(Type, new StageData
            {
                Stage = stage,
                Wave = wave
            });
        }

        private void OnTimerCompleted(ITimer _)
        {
            _timer.OnComplete -= OnTimerCompleted;
            OnWaveFailed(CurrentStage);
        }

        private void OnWaveStarted(IStage stage)
        {
            StageChanged?.Invoke(this);
        }

        private void OnWaveFailed(IStage stage)
        {
            StageFailed?.Invoke(this);

            if (CurrentStage.StageConfig.IsTimer)
            {
                _timer.Pause();
            }
        }

        private void OnWaveCompleted(IStage stage)
        {
            UpdateProgress();
            WaveCompleted?.Invoke(this);
        }

        private void OnStageCompleted(IStage stage)
        {
            var config = GetCurrentStageConfig(Type);

            if (stage.StageData.Wave + 1 >= config.Waves.Length)
            {
                var nextConfig = GetNextLevelConfig();
                if (nextConfig == null)
                {
                    Logger.Error($"no next config");
                    return;
                }

                var data = stage.StageData;
                data.Stage++;
                data.Wave = 0;
                var newStage = _stageFactory.Load(nextConfig, data);
                SetCurrentStage(newStage);
            }

            if (CurrentStage.StageConfig.IsTimer)
            {
                _timer.Pause();
            }

            _analyticsManager.Send(EEventType.StageFinish, GetStageEventParameters());
            StageCompleted?.Invoke(this);

            if (Type != EStageType.Default)
            {
                _stageModel.SetType(EStageType.Default);
            }
        }

        private Dictionary<string, object> GetStageEventParameters()
        {
            var data = CurrentStage.StageData;
            return new Dictionary<string, object>()
            {
                { "level_number", data.Stage + 1 },
                { "level_name", $"Level {data.Stage + 1}" },
                { "level_count", data.Count },
                { "level_diff", "normal" },
                { "level_loop", 1 },
                { "level_random", 0 },
                { "level_type", Type },
                { "game_mode", "classic" }
                //{} // NOTE: Add time 
            };
        }

        public List<ResourceData> GetRewardForStage(EStageType stageType)
        {
            var rewards = new List<ResourceData>();

            var booster = _boostersModel.Boosters[BoostersIDs.INCOME];
            var rewardFactorForBooster = 1 + (booster.Progress.IsActive ? booster.BlessingValue : 0);

            foreach (var reward in GetCurrentStageConfig(stageType).RewardsConfig.RewardForWave)
            {
                var resource = reward.Resource;
                var value = reward.Quantity;
                value *= (Progress(stageType).Stage!.Value + .5f) * REWARD_FACTOR_BY_LEVEL *
                         rewardFactorForBooster;

                rewards.Add(new ResourceData(resource, value));
            }

            return rewards;
        }
    }
}