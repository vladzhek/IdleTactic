using System.Collections.Generic;
using UnityEngine;

namespace Slime.Configs.Effects
{
    [CreateAssetMenu(fileName = PATH, menuName = "Assets/EffectsHolder", order = 0)]
    public class EffectsHolder : ScriptableObject
    {
        private const string PATH = "EffectsHolder";
        
        public static string Path => PATH;

        [SerializeField] private EffectEntry[] _effectEntries;

        public IEnumerable<EffectEntry> EffectEntries => _effectEntries;
    }
}