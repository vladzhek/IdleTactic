using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.UI.Common;
using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using Utils;

namespace Slime.UI.Footer.Dungeons
{
    public class DungeonsEntryView : View<DungeonsViewModel>
    {
        [SerializeField] private DungeonLayoutElement _bossRushElement;
        [SerializeField] private DungeonLayoutElement _goldRushElement;

        private float _timeDuration;

        public override UILayer Layer => UILayer.Middleground;

        protected override void Start()
        {
            base.Start();

            SetDungeonLevel();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            ViewModel.KeysAmountChange += OnDungeonKeysValue;
            ViewModel.DungeonStageCompleted += OnStageCompleted;
            _bossRushElement.LoadDungeon += StartBossRush;
            _goldRushElement.LoadDungeon += StartBossRush;

            SetDungeonLevel();
            SetDungeonRewards();

            OnDungeonKeysValue(EResource.BossRushCurrency, ViewModel.GetKeysAmount(EStageType.BossRush));
            OnDungeonKeysValue(EResource.GoldRushCurrency, ViewModel.GetKeysAmount(EStageType.GoldRush));
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();

            ViewModel.KeysAmountChange -= OnDungeonKeysValue;
            ViewModel.DungeonStageCompleted -= OnStageCompleted;
            _bossRushElement.LoadDungeon -= StartBossRush;
            _goldRushElement.LoadDungeon -= StartBossRush;
        }

        private void StartBossRush(EStageType type)
        {
            ViewModel.StartDungeon(type);
        }

        private void OnDungeonKeysValue(EResource id, float value)
        {
            _goldRushElement.SetResourceValue(SetKeysValue(EStageType.GoldRush));
            _bossRushElement.SetResourceValue(SetKeysValue(EStageType.BossRush));

            UpdateButtons(id, value);
        }

        private (int, bool) SetKeysValue(EStageType type)
        {
            var value = ViewModel.GetKeysAmount(type) > 0
                ? ViewModel.GetKeysAmount(type)
                : ViewModel.GetAdsAmount(type);

            var isAds = ViewModel.GetKeysAmount(type) == 0;

            return (value, isAds);
        }

        private void UpdateButtons(EResource resource, float value)
        {
            if (resource == EResource.BossRushCurrency)
            {
                var sprite = ViewModel.GetIcon(value > 0 
                    ? EResource.BossRushCurrency.ToSpriteID() 
                    : SpritesIDs.ADS_ICON);
                
                _bossRushElement.UpdateButton(value, sprite, ViewModel.IsAdsAvailable(EStageType.BossRush));
            } 
            
            else if (resource == EResource.GoldRushCurrency)
            {
                var sprite = ViewModel.GetIcon(value > 0 
                    ? EResource.GoldRushCurrency.ToSpriteID() 
                    : SpritesIDs.ADS_ICON);
                
                _goldRushElement.UpdateButton(value, sprite, ViewModel.IsAdsAvailable(EStageType.GoldRush));
            }
        }

        private void OnStageCompleted(EStageType id)
        {
            SetDungeonLevel();
        }

        private void SetDungeonLevel()
        {
            _bossRushElement.SetDungeonLevel($"{ViewModel.GetDungeonLevel(EStageType.BossRush)}");;
            _goldRushElement.SetDungeonLevel($"{ViewModel.GetDungeonLevel(EStageType.GoldRush)}");
        }

        private void SetDungeonRewards()
        {
            _bossRushElement.SetDungeonRewards(ViewModel.GetDungeonReward(EStageType.BossRush).ToString());
            _goldRushElement.SetDungeonRewards(ViewModel.GetDungeonReward(EStageType.GoldRush).ToString());
        }
    }
}