using System;
using System.Linq;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Data.Enums;
using Slime.UI.Attributes;
using UI.Base.MVVM;
using UnityEngine;
using Utils;
using Utils.Time;

namespace Slime.UI.Footer.Dungeons
{
    public class DungeonsViewModel : ViewModel
    {
        private const int SECONDS_IN_A_DAY = 86400;
        
        public event Action<EStageType> DungeonStageCompleted;

        private readonly IDungeonsUIModel _dungeonsUIModel;
        private readonly ISpritesModel _spritesModel;
        private readonly IStageController _stageController;
        private readonly UIManager _uiManager;
        private readonly IResourcesModel _resourcesModel;
        
        public DungeonsViewModel(IDungeonsUIModel dungeonsUIModel, 
            ISpritesModel spritesModel, 
            IStageController stageController,
            UIManager uiManager,
            IResourcesModel resourcesModel)
        {
            _dungeonsUIModel = dungeonsUIModel;
            _spritesModel = spritesModel;
            _stageController = stageController;
            _uiManager = uiManager;
            _resourcesModel = resourcesModel;
        }
        
        public event Action<EResource, float> KeysAmountChange
        {
            add => _resourcesModel.OnChange += value;
            remove => _resourcesModel.OnChange -= value;
        }
        
        public override void OnSubscribe()
        {
            base.OnSubscribe();

            _stageController.StageStarted += OnLoadDungeon;
            _stageController.WaveCompleted += OnDungeonCompleted;
        }

        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();

            _stageController.StageStarted -= OnLoadDungeon;
            _stageController.WaveCompleted -= OnDungeonCompleted;
        }

        private void OnLoadDungeon(IStageController stageController)
        {
            if (stageController.Type == EStageType.Default)
            {
                return;
            }

            _uiManager.Open<AttributesView>();
        }

        private void OnDungeonCompleted(IStageController stageController)
        {
            if(stageController.Type == EStageType.Default) return;
            
            DungeonStageCompleted?.Invoke(stageController.Type);

            // TODO: show reward here
            //_rewardsUIModel.Open(new ResourceData());
        }

        public void StartDungeon(EStageType eBattleType)
        {
            _dungeonsUIModel.LoadDungeon(eBattleType);
        }

        public int GetKeysAmount(EStageType type)
        {
            return _dungeonsUIModel.GetAvailableKeysValue(type);
        }

        public int GetAdsAmount(EStageType type)
        {
            return _dungeonsUIModel.GetAvailableAdsValue(type);
        }

        public bool IsAdsAvailable(EStageType type)
        {
            return _dungeonsUIModel.IsDailyDungeonAdsAvailable(type);
        }

        public Sprite GetIcon(string id)
        {
            return _spritesModel.Get(id);
        }

        public int GetDungeonLevel(EStageType type)
        {
            return _dungeonsUIModel.GetDungeonLevel(type) + 1;
        }

        public int GetDungeonReward(EStageType type)
        {
            var value = 0f;
            foreach (var reward in _stageController.GetRewardForStage(type))
            {
                value += reward.Quantity;
            }
            
            return (int)value;
        }

        public static string GetTimeToNewKeys()
        {
            return FormatTime.HoursStringFormat(SECONDS_IN_A_DAY - (int)DateTime.Now.TimeOfDay.TotalSeconds);
        }
    }
}