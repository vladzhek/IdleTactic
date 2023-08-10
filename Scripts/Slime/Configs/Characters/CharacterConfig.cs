using Slime.Configs.Abilities;
using Slime.Configs.Abstract;
using Slime.Data.Characters;
using UnityEngine;

namespace Slime.Configs.Characters
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 11)]
    public class CharacterConfig : ItemConfig<CharacterData>
    {
        public override CharacterData Item 
        {
            get 
            {
                var item = base.Item;
                item.Abilities = _abilities != null ? _abilities.Items : null;
                item.Skill = _skill != null ? _skill.Item : null;
                return item;
            }
        }

        [Space]
        [SerializeField] private AbilitiesConfig _abilities;
        [SerializeField] private CharacterSkillConfig _skill;
        
        private const string ENTITY = "Character";
    }
}