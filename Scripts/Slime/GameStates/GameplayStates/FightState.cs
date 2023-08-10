using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.Data.Products;
using Slime.Data.Triggers;
using Slime.Services.EffectsService;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.GameStates.GameplayStates
{
    [UsedImplicitly]
    public class FightState : State<EGameplayTriggers>, IUpdatable
    {

        private readonly IPlayer _player;
        private readonly ICompanion _companion;
        private readonly IStageModel _stageModel;
        private readonly IRewardsModel _rewardModel;
        private readonly IRewardsUIModel _rewardsUIModel;
        private readonly IUnitsModel _unitsModel;
        private readonly IEffectsService _effectsService;
        private readonly IStageController _stageController;

        protected FightState(GameplayStateMachine stateMachine,
            IPlayer player,
            ICompanion companion,
            IStageModel stageModel,
            IRewardsModel rewardModel,
            IUnitsModel unitsModel,
            IEffectsService effectsService,
            IRewardsUIModel rewardsUIModel,
            IStageController stageController
        ) : base(stateMachine)
        {
            _player = player;
            _companion = companion;
            _stageModel = stageModel;
            _rewardModel = rewardModel;
            _rewardsUIModel = rewardsUIModel;
            _unitsModel = unitsModel;
            _effectsService = effectsService;
            _stageController = stageController;
        }

        private bool IsStageCycled => _stageModel.Type == EStageType.Default &&
                                      (_stageModel.Get(EStageType.Default).IsCycled ?? false);

        public override void OnEnter()
        {
            _unitsModel.OnEnemyDied += OnEnemyDied;
            _player.Unit.Die += OnPlayerDies;
            _stageController.StageStarted += OnStageStarted;
            _stageController.StageCompleted += OnStageCompleted;
            _stageController.StageFailed += OnStageFailed;
            _stageController.WaveCompleted += OnWaveCompleted;
            _stageModel.OnCycleChange += OnCycleModeChanged;
            _stageModel.OnChange += OnStageChange;


            SetUnitsActive(true);
            _stageModel.SetScreenHidden(false);
        }

        public override void OnExit()
        {
            _unitsModel.OnEnemyDied -= OnEnemyDied;
            _player.Unit.Die -= OnPlayerDies;
            _stageController.StageStarted -= OnStageStarted;
            _stageController.StageCompleted -= OnStageCompleted;
            _stageController.StageFailed -= OnStageFailed;
            _stageController.WaveCompleted -= OnWaveCompleted;
            _stageModel.OnCycleChange -= OnCycleModeChanged;
            _stageModel.OnChange -= OnStageChange;

            SetUnitsActive(false);
        }

        public void OnUpdate(float deltaTime)
        {
            _player.Unit.OnUpdate(deltaTime);
            _companion.OnUpdate(deltaTime);

            foreach (var enemyUnit in _unitsModel.EnemyUnits.Collection)
            {
                enemyUnit.OnUpdate(deltaTime);
            }
        }

        private void SetUnitsActive(bool isActive)
        {
            _player.Unit.SetActive(isActive);

            foreach (var enemyUnit in _unitsModel.EnemyUnits.Collection)
            {
                enemyUnit.SetActive(isActive);
            }
        }

        private void OnWaveCompleted(IStageController stageController)
        {
            StateMachine.FireTrigger(EGameplayTriggers.LoadStage);

            if (!IsStageCycled)
            {
                var rewards = _stageController.GetRewardForStage(stageController.Type);

                if (EStageType.Default != _stageController.CurrentStage.StageData.Type)
                {
                    foreach (var reward in rewards)
                    {
                        _rewardsUIModel.Open(new ResourceData(reward.Resource, reward.Quantity));
                    }
                }

                var request = new EffectRequest(EffectIDs.GOLD, Vector3.zero).SetLifetime(0);
                _effectsService.RequestEffect(request).SetPromise(_rewardModel
                    .AddRewardForStage(rewards));
            }
        }

        private void OnStageStarted(IStageController stageFactory)
        {
            var data = _stageModel.Get(stageFactory.Type);
            data.Count ??= 0;
            data.Count++;
        }

        private void OnStageCompleted(IStageController stageFactory)
        {
            _stageModel.SetScreenHidden(true);
            StateMachine.FireTrigger(EGameplayTriggers.LoadStage);
        }

        private void OnStageFailed(IStageController stageFactory)
        {
            if (stageFactory.Type == EStageType.Default)
            {
                _stageModel.SetCycled(stageFactory.CurrentStage.IsLastWave && !_stageModel.IsBattleCycled);
            }
            else
            {
                _stageModel.SetType(EStageType.Default);
            }

            _stageModel.SetScreenHidden(true);
            StateMachine.FireTrigger(EGameplayTriggers.PlayerCreation);
        }

        private void OnEnemyDied(IUnit enemy)
        {
            var rewardsForUnits = _stageController.CurrentStage.StageConfig.RewardsConfig.RewardForUnits;
            if (!rewardsForUnits.TryGetValue(enemy.ID, out var rewards))
            {
                Logger.Warning($"No rewards for {enemy.ID}");
                return;
            }

            var resourceData = rewards as ResourceData[] ?? rewards.ToArray();
            foreach (var data in resourceData)
            {
                // NOTE: should just check for boss reward?
                if (data.Resource == EResource.CharacterUpgradeCurrency)
                {
                    var value = data.Quantity;
                    _rewardsUIModel.Open(new ResourceData(data.Resource, value));
                }
            }

            // NOTE: check if there is softCurrency in reward?
            var request = new EffectRequest(EffectIDs.GOLD, enemy.Avatar.ProjectileOrigin.position).SetLifetime(1)
                .SetTarget(enemy.Avatar.ProjectileOrigin.position);
            _effectsService.RequestEffect(request)
                .SetPromise(_rewardModel.AddRewardForUnit(resourceData));
        }

        private void OnCycleModeChanged(bool isCycled)
        {
            if (!isCycled)
            {
                _stageModel.SetScreenHidden(true);

                StateMachine.FireTrigger(EGameplayTriggers.PlayerCreation);
            }
        }

        private void OnPlayerDies(IUnit obj)
        {
            //В DelayReset стоит переход в state PlayerCreation, так же этот переход дублирается при StageFailed
            //_ = DelayReset();
        }

        private async UniTask DelayReset()
        {
            await UniTask.Delay(2000);
            await UniTask.SwitchToMainThread();

            _stageModel.SetScreenHidden(true);
            StateMachine.FireTrigger(EGameplayTriggers.PlayerCreation);
        }

        private void OnStageChange(EStageType id)
        {
            _stageModel.SetScreenHidden(true);
            StateMachine.FireTrigger(EGameplayTriggers.PlayerCreation);
        }
    }
}