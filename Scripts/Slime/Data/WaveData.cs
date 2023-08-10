using System.Collections.Generic;
using Slime.Data.IDs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Assets/Wave", order = 0)]
    public class WaveData : ScriptableObject
    {
        [SerializeField, ValueDropdown(nameof(UnitsID))]
        private string[] _units;

        private IEnumerable<string> UnitsID => EnemyArchetypeIDs.Values;

        public string[] Units => _units;
    }
}