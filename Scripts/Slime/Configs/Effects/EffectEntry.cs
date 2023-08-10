using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.AbstractLayer;
using Slime.Data.IDs;
using Slime.Services.EffectsService;
using UnityEngine;

namespace Slime.Configs
{
    [Serializable]
    public struct EffectEntry
    {
        [SerializeField, ValueDropdown(nameof(IDs))]
        private string _id;

        [SerializeField, AssetSelector(Paths = "Assets/Prefabs/Effects")]
        private Effect _effect;

        public string ID => _id;
        public IEffect Effect => _effect;

        private IEnumerable<string> IDs => EffectIDs.Values;
    }
}