using System.Linq;
using Slime.Data.IDs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Slime.Configs
{
    [CreateAssetMenu(fileName = "UnitsConfig", menuName = "Assets/Units/Config", order = 0)]
    public class UnitsConfig : ScriptableObject
    {
        [SerializeField] private UnitConfig[] _unitsConfigs;

        public UnitConfig[] UnitsConfigs => _unitsConfigs;

        [Button("Add all units")]
        public void AddAllUnits()
        {
            var ids = CharacterIDs.Values.Concat(EnemyArchetypeIDs.Values).Concat(CompanionIDs.Values).ToArray();
            var oldList = _unitsConfigs.ToList();
            var newConfigs = new UnitConfig[ids.Length];
            for (int i = 0; i < newConfigs.Length; i++)
            {
                if (oldList.Any(x => x.UnitID.Equals(ids[i])))
                {
                    newConfigs[i] = oldList.First(x => x.UnitID.Equals(ids[i]));
                    continue;
                }

                newConfigs[i] = new UnitConfig(ids[i]);
            }

            _unitsConfigs = newConfigs;
        }
    }
}