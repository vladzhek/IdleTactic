using System.Collections.Generic;
using System.Linq;
using Data.Units;
using Sirenix.OdinInspector;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Configs;
using Slime.Data.IDs;
using UnityEngine;

namespace Slime.Configs.Units
{
    [CreateAssetMenu(fileName = "UnitsAvatars", menuName = "Assets/UnitsAvatars", order = 0)]
    public class UnitsAvatars : ScriptableObject, IUnitsAvatars
    {
        [SerializeField] private UnitInfo[] _units;
        [SerializeField] private bool _isPlayer;

        public Dictionary<string, IUnitAvatar> GetUnitsAvatars() => _units.ToDictionary(x => x.ID, x => x.UnitAvatar);

        [Button("Add all units")]
        public void AddAllUnits()
        {
            var ids = _isPlayer ? CharacterIDs.Values.Concat(CompanionIDs.Values).ToArray() : EnemyArchetypeIDs.Values.ToArray();
            var oldList = _units.ToList();
            var newConfigs = new UnitInfo[ids.Length];
            for (var i = 0; i < newConfigs.Length; i++)
            {
                if (oldList.Any(x => x.ID.Equals(ids[i])))
                {
                    newConfigs[i] = oldList.First(x => x.ID.Equals(ids[i]));
                    continue;
                }

                newConfigs[i] = new UnitInfo(ids[i]);
            }

            _units = newConfigs;
        }
    }
}