using System.Collections.Generic;
using System.Linq;
using Slime.Data.Abilities;
using UnityEngine;

namespace Slime.Configs.Abilities
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 0)]
    public class AbilitiesConfig : ScriptableObject
    {
        public IEnumerable<AbilityData> Items => from item in _items select item.Item;
        
        [SerializeField] private AbilityConfig[] _items = {};
        
        private const string ENTITY = "Abilities";
    }
}