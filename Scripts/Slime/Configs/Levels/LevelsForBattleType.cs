using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.AbstractLayer.Configs;
using Slime.Data;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.Levels;
using UnityEngine;
using UnityEngine.Serialization;

namespace Slime.Configs.Levels
{
    [Serializable]
    public class LevelsForBattleType
    {
        [FormerlySerializedAs("_battleType")] [SerializeField] private EStageType _stageType;

        [SerializeField, AssetSelector(Paths = "Assets/Resources")]
        private StageConfig[] _levelConfigs;

        public EStageType StageType => _stageType;
        public IStageConfig[] LevelConfigs => _levelConfigs;

        private IStageConfig[] GetLevelsConfigs()
        {
            var result = new IStageConfig[_levelConfigs.Length];
            for (var i = 0; i < _levelConfigs.Length; i++)
            {
                result[i] = _levelConfigs[i];
            }

            return result;
        }
    }
}