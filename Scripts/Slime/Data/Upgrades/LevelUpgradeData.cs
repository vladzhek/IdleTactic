using System;
using Slime.Data.Enums;

namespace Slime.Data.Upgrades
{
    [Serializable]
    public class LevelUpgradeData
    {
        public float StartValue;
        public float FactorPerLevel;
        public EModificationType ModificationType;
    }
}