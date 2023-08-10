using Slime.Data.Abilities;
using UnityEngine;

namespace Slime.Configs.Abilities
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 11)]
    public class AbilityConfig : ScriptableObject
    {
        public AbilityData Item => _item;
        
        [SerializeField] private AbilityData _item;

        private const string ENTITY = "Ability";
    }
}