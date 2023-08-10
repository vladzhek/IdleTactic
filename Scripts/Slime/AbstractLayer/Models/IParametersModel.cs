using System;
using System.Collections.Generic;
using Slime.Data.Enums;

namespace Slime.AbstractLayer.Models
{
    public interface IParametersModel
    {
        public event Action OnChange;
        public IDictionary<EAttribute, float> Get();
        public float Get(EAttribute attribute);
    }
}