using System.Collections.Generic;
using System.Linq;
using Slime.Data.Enums;
using UnityEngine;

namespace Slime.Configs
{
    [CreateAssetMenu(fileName = "DailyLimitsSettings", menuName = "Assets/DailyLimitsSettings", order = 0)]
    public class DailyLimitsSettings : ScriptableObject
    {
        public const string PATH = "DailyLimitsSettings";

        [SerializeField] private List<DungeonKeysLimits> _dungeonKeysLimits;
        
        public Dictionary<EStageType, DungeonKeysLimits> GetDungeonLimitsDictionary()
        {
            return _dungeonKeysLimits.ToDictionary(x => x.StageType, x => x);
        }
    }
}