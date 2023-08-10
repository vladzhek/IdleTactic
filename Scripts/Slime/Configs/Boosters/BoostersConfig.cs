using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Slime.Data.Boosters;
using UnityEngine;
using UnityEngine.Serialization;

namespace Slime.Configs.Boosters
{
    [CreateAssetMenu(fileName = "BoostersConfig", menuName = "Assets/Configs/BoostersConfig", order = 0)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BoostersConfig : ScriptableObject
    {
        public static readonly string PATH = $"{Data.Constants.System.CONFIG_PATH}Boosters";
        
        [FormerlySerializedAs("_bustersData"), SerializeField] private BoosterData[] _boostersData;

        public Dictionary<string, BoosterData> GetDictionary()
        {
            return _boostersData.ToDictionary(x => x.ID, x => x);
        }
    }
}