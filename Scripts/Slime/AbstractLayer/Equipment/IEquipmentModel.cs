using System;
using System.Collections.Generic;
using Slime.Data.Enums;

namespace Slime.AbstractLayer.Equipment
{
    public interface IEquipmentModel
    {
        public event Action OnChange;
        
        public IDictionary<EAttribute, float> ParameterModifiers { get; }
    }
}