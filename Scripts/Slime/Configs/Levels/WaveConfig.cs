using System;
using Data.Units;
using Slime.AbstractLayer.Configs;
using Slime.Configs.Units;
using UnityEngine;

namespace Slime.Levels
{
    [Serializable]
    public class WaveConfig : IWaveConfig
    {
        [SerializeField] private UnitsAvatars _unitsAvatars;
        [SerializeField] private string[] _units;
        public IUnitsAvatars UnitsAvatars => _unitsAvatars;
        public string[] Units => _units;

        public WaveConfig(string[] units, UnitsAvatars unitsAvatars)
        {
            _unitsAvatars = unitsAvatars;
            _units = units;
        }
    }
}