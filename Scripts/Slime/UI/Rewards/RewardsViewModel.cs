using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using Slime.Data.IDs;
using UI.Base.MVVM;
using Utils.Extensions;
using Utils.Time;

namespace Slime.UI.Rewards
{
    public class RewardsViewModel : ViewModel
    {
        public override void OnSubscribe()
        {
            base.OnSubscribe();

            if (Timer != null)
            {
                Timer.OnComplete += OnTimerCompleted;
            }
        }
        
        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            if (Timer != null)
            {
                Timer.OnComplete -= OnTimerCompleted;
            }
        }

        public IEnumerable<RewardsLayoutData> Data =>
            from item in _rewardsUIModel.Get()
            select new RewardsLayoutData(
                _spritesModel.Get(item.Resource.ToSpriteID()),
                item.Quantity.ToMetricPrefixString());

        public void CloseView()
        {
            _rewardsUIModel.Close();
        }

        // private
        
        private readonly IRewardsUIModel _rewardsUIModel;
        private readonly ISpritesModel _spritesModel;
        private readonly TimerService _timerService;
        
        private RewardsViewModel(IRewardsUIModel rewardsUIModel, ISpritesModel spritesModel,
            TimerService timerService)
        {
            _rewardsUIModel = rewardsUIModel;
            _spritesModel = spritesModel;
            _timerService = timerService;
        }

        private ITimer Timer => _timerService.Timers.GetValueOrDefault(TimerIDs.REWARD_POPUP_CLOSE);
        
        private void OnTimerCompleted(ITimer _)
        {
            CloseView();
        }
    }
}