using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Slime.AbstractLayer.Models;
using Slime.Data.Constants;
using Slime.Data.IDs;
using Slime.Data.Products;
using Slime.UI;
using Slime.UI.Rewards;

namespace Slime.Models
{
    public class RewardsUIModel : IRewardsUIModel
    {
        #region IRewardsUIModel implementation
        
        public IEnumerable<ResourceData> Get()
        {
            yield return _data;
        }

        public UniTask Open(ResourceData data)
        {
            _taskCompletionSource?.TrySetResult();
            _taskCompletionSource = new UniTaskCompletionSource();

            _data = data;
            _uiManager.OpenWithTimer<RewardsView>(
                TimerIDs.REWARD_POPUP_CLOSE, 
                Values.REWARD_POPUP_CLOSE_DELAY);

            return _taskCompletionSource.Task;
        }

        public void Close()
        {
            _uiManager.Close<RewardsView>();
            
            _taskCompletionSource?.TrySetResult();
            _taskCompletionSource = null;
            _data = null;
        }
        
        #endregion
        
        // private

        private readonly UIManager _uiManager;

        private ResourceData _data;
        private UniTaskCompletionSource _taskCompletionSource;
        
        private RewardsUIModel(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
    }
}