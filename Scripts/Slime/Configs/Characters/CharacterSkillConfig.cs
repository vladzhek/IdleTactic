using Slime.Configs.Abstract;
using Slime.Data.Characters;
using UnityEngine;

namespace Slime.Configs.Characters
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 11)]
    public class CharacterSkillConfig : ItemConfig<CharacterSkillData>
    { 
        private const string ENTITY = "CharacterSkill";
    }
}