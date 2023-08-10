using Slime.Configs.Abstract;
using Slime.Data.Skills;
using UnityEngine;

namespace Slime.Configs.Skills
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 11)]
    public class SkillConfig : ItemConfig<SkillData>
    {
        private const string ENTITY = "Skill";
    }
}