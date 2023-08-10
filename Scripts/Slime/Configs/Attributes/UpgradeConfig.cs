using System;
using Slime.AbstractLayer.Configs;
using Slime.Data.Enums;
using Slime.Data.Upgrades;
using UnityEngine;
using UnityEngine.Serialization;

namespace Slime.Configs.Attributes
{
    // TODO: remove
    [CreateAssetMenu(fileName = "UpgradesConfig", menuName = "Assets/Upgrades/GenericConfig", order = 0)]
    public class UpgradeConfig : ScriptableObject, IUpgradeConfig
    {
        [FormerlySerializedAs("_valuesData")] [SerializeField] private LevelUpgradeData _valuesUpgradeData;
        [FormerlySerializedAs("_priceData")] [SerializeField] private LevelUpgradeData _priceUpgradeData;
        [SerializeField] private int _maxLevel;

        public int MaxLevel => _maxLevel;

        public float GetValueForLevel(int level)
        {
            return GetValue(_valuesUpgradeData, level);
        }

        public float GetPriceForLevel(int level)
        {
            return GetValue(_priceUpgradeData, level);
        }

        private float GetValue(LevelUpgradeData levelUpgradeData, int level)
        {
            return levelUpgradeData.ModificationType switch
            {
                EModificationType.Add => levelUpgradeData.StartValue + levelUpgradeData.FactorPerLevel * level,
                EModificationType.Multiply => levelUpgradeData.StartValue * levelUpgradeData.FactorPerLevel * level,
                EModificationType.Percentage => levelUpgradeData.StartValue * (float)Math.Pow(1 + levelUpgradeData.FactorPerLevel / 100, level - 1),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}