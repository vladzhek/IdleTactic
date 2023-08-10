using System.Collections.Generic;
using Slime.AbstractLayer.Models;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using Slime.UI.Tutorial;
using UI.Base.MVVM;

namespace Slime.UI.Popups.Summon
{
    public class SummonResultViewModel : ViewModel
    {
        // public

        public ESummonType Type => _summonResultUIModel.Type;
        
        public IEnumerable<ISummonable> Data => _summonResultUIModel.Items;
        
        public void CloseView()
        {
            _uiManager.Close<SummonResultView>();

            if (_tutorialModel.Stage == ETutorialStage.Inventory)
            {
                _uiManager.Open<TutorialView>();
            }
        }

        // private
        
        private readonly ISummonResultUIModel _summonResultUIModel;
        private readonly UIManager _uiManager;
        private readonly ITutorialModel _tutorialModel;
        
        private SummonResultViewModel(
            ISummonResultUIModel summonResultUIModel,
            UIManager manager,
            ITutorialModel tutorialModel)
        {
            _summonResultUIModel = summonResultUIModel;
            _uiManager = manager;
            _tutorialModel = tutorialModel;
        }
    }
}