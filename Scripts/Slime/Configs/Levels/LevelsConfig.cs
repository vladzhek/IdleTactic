using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer.Configs;
using Slime.Data;
using Slime.Data.Enums;
using Slime.Data.IDs;
using UnityEngine;

namespace Slime.Configs.Levels
{
    [CreateAssetMenu(fileName = "Levels", menuName = "Assets/Configs/Levels", order = 0)]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField] private LevelsForBattleType[] _levels;

        public Dictionary<EStageType, IStageConfig[]> Levels => _levels.ToDictionary(x => x.StageType, x => x.LevelConfigs);
    }
}