using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using Slime.Data.Summon;
using UI.Base.MVVM;

namespace Slime.UI.Popups.Summon
{
    public class SummonInfoViewModel : ViewModel
    {
        public IEnumerable<SummonInfoData> Data => from rarity in ERarityExtensions.Implemented
            select new SummonInfoData(rarity, SummonData.GetProbability(rarity));
        
        public void CloseView()
        {
            _summonInfoUIModel.Close();
        }
        
        // private

        private readonly ISummonModel _summonModel;
        private readonly ISummonInfoUIModel _summonInfoUIModel;
        
        private SummonData SummonData => _summonModel.Get(_summonInfoUIModel.Type).Item1;
        
        private SummonInfoViewModel(ISummonModel summonModel,
            ISummonInfoUIModel summonInfoUIModel)
        {
            _summonModel = summonModel;
            _summonInfoUIModel = summonInfoUIModel;
        }
    }
}