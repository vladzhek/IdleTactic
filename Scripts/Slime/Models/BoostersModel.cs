using System;
using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Configs.Boosters;
using Slime.Data.Boosters;
using Slime.Data.IDs;
using Slime.Data.IDs.Boosters;
using Slime.Gameplay.Battle.Boosters;
using Slime.Models.Abstract;
using UnityEngine;
using Utils.Time;

namespace Slime.Models
{
    public class BoostersModel : BaseProgressModel, IBoostersModel
    {
        public event Action OnChange;
        public event Action<string> Activated;
        public event Action<string> Deactivated;

        private readonly ISkillsManager _skillsManager;
        private readonly IRewardsModel _rewardsModel;
        private readonly TimerService _timerService;
        private readonly IAdsService _adsService;

        private BoostersConfig _boostersConfig;
        public Dictionary<string, IBooster> Boosters { get; } = new();

        public BoostersModel(IGameProgressModel progressModel, ISkillsManager skillsManager, IRewardsModel rewardsModel,
            TimerService timerService, IAdsService adsService) : base(progressModel)
        {
            _skillsManager = skillsManager;
            _rewardsModel = rewardsModel;
            _timerService = timerService;
            _adsService = adsService;
        }

        public override void Dispose()
        {
            base.Dispose();
            SaveTimeToDeactivation();
        }
        
        public async void ActivateBoosterForAds(string id)
        {
            if (await _adsService.ShowAd(AdPlacementIDs.BOOSTER))
            {
                ActivateBooster(id);
            }
        }

        protected override void OnProgressLoaded()
        {
            // TODO: can we do this on demand?
            UpdateState();
        }

        private void UpdateState()
        {
            _boostersConfig ??= Resources.Load<BoostersConfig>(BoostersConfig.PATH);
            var boosters = _boostersConfig.GetDictionary().Values;
            foreach (var data in boosters)
            {
                var id = data.ID;
                var progress = GetProgressData(id);

                IBooster booster = id switch
                {
                    BoostersIDs.COOLDOWN => new SkillCooldownBlessingBooster(progress, data, _skillsManager,
                        _timerService),
                    BoostersIDs.ATK => new DamageBlessingBooster(progress, data, _timerService),
                    BoostersIDs.INCOME => new GoldBlessingBooster(progress, data, _timerService),
                    _ => null
                };

                if (progress.IsActive)
                {
                    booster?.Activate();
                }

                Boosters.Add(data.ID, booster);
            }
            
            OnChange?.Invoke();
        }

        private void ActivateBooster(string id)
        {
            if (Boosters.TryGetValue(id, out IBooster booster))
            {
                if (!booster.Progress.IsActive)
                {
                    booster.Progress.Amount++;
                    booster.Upgrade();
                    booster.Activate();
                    Activated?.Invoke(id);

                    booster.OnDeactivate += DeactivateBooster;
                }
            }
        }

        private void DeactivateBooster(string id)
        {
            Boosters[id].OnDeactivate -= DeactivateBooster;

            Deactivated?.Invoke(id);
        }

        private BoosterProgressData GetProgressData(string id)
        {
            var data = GameData.BoosterData;
            if (data == null)
            {
                data = new Dictionary<string, BoosterProgressData>();
                GameData.BoosterData = data;
            }

            if (data.TryGetValue(id, out var progressData))
            {
                return progressData;
            }

            progressData = new BoosterProgressData(id, _boostersConfig.GetDictionary()[id].StartingValue);
            GameData.BoosterData.Add(id, progressData);

            return progressData;
        }

        private void SaveTimeToDeactivation()
        {
            // TODO: this doesn't work
            foreach (var booster in 
                     from booster in Boosters?.Values 
                     where booster?.Progress?.IsActive ?? false 
                     select booster)
            {
                booster.Progress.TimeToDeactivation = (int)_timerService.Timers[booster.Data.ID].TimeLeft;
            }
        }
    }
}