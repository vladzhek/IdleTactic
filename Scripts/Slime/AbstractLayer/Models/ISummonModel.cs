using System;
using System.Collections.Generic;
using Slime.AbstractLayer.Equipment;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using Slime.Data.Summon;

namespace Slime.AbstractLayer.Models
{
    public interface ISummonModel : IUpgradableModel<SummonBaseData>
    {
        public event Action OnChange;
        public event Action<ESummonType> OnItemChange;
        public event Action<ESummonType> OnSummon;
        public (SummonData,SummonAdData) Get(ESummonType type);
        public IEnumerable<ISummonable> Summon(ESummonType type, ESummonCategory category);
    }
}