using System.Collections.Generic;
using Slime.AbstractLayer.Models;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using Slime.UI;
using Slime.UI.Popups.Summon;

namespace Slime.Models
{
    public class SummonResultUIModel : ISummonResultUIModel
    {
        public ESummonType Type { get; private set; }
        public IEnumerable<ISummonable> Items { get; private set; }
        
        public void Open(ESummonType type, IEnumerable<ISummonable> items)
        {
            Type = type;
            Items = items;
            
            _uiManager.Open<SummonResultView>();
        }
        
        // private

        private readonly UIManager _uiManager;

        private SummonResultUIModel(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
    }
}