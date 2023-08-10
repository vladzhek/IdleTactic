using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using Slime.UI;
using Slime.UI.Popups.Summon;
using Utils;

namespace Slime.Models
{
    public class SummonInfoUIModel : ISummonInfoUIModel
    {
        public ESummonType Type { get; private set; }
        
        public void Open(ESummonType type)
        {
            Type = type;
            
            _uiManager.Open<SummonInfoView>();
        }

        public void Close()
        {
            Logger.Log();
            _uiManager.Close<SummonInfoView>();
        }

        // private

        private readonly UIManager _uiManager;

        private SummonInfoUIModel(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
    }
}