using Slime.AbstractLayer.Models;
using UI.Base.MVVM;

namespace Slime.UI.Popups
{
    public class OfflineIncomeViewModel : ViewModel
    {
        private readonly UIManager _uiManager;
        private readonly IOfflineIncomeModel _offlineIncomeModel;


        public OfflineIncomeViewModel(UIManager uiManager, IOfflineIncomeModel offlineIncomeModel)
        {
            _uiManager = uiManager;
            _offlineIncomeModel = offlineIncomeModel;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            _offlineIncomeModel.AddReward(_offlineIncomeModel.OfflineIncomeValue);
        }

        public void CloseView()
        {
            _uiManager.Close<OfflineIncomePopupView>();
        }

        public int GetRewardValue()
        {
            return _offlineIncomeModel.OfflineIncomeValue;
        }

        public void ShowAd()
        {
            _offlineIncomeModel.ShowAd();
            CloseView();
        }
    }
}