using System.Collections.Generic;
using Slime.Data.Abstract;
using Slime.Data.Enums;

namespace Slime.AbstractLayer.Models
{
    public interface ISummonResultUIModel
    {
        // NOTE: OnChange? (for autosummon)
        public ESummonType Type { get; }
        public IEnumerable<ISummonable> Items { get; }
        void Open(ESummonType type, IEnumerable<ISummonable> items);
    }
}