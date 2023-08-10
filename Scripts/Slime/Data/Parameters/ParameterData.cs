using System;
using Slime.Data.Enums;
using UnityEngine;

namespace Slime.Data.Parameters
{
    [Serializable]
    public class ParameterData
    {
        [SerializeField] private EAttribute _type;
        [SerializeField] private float _value;
        
        public EAttribute Type => _type;
        public float Value => _value;
    }
}