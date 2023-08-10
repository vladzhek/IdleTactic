using Slime.Configs.Abstract;
using Slime.Data.Skills;
using UnityEngine;

namespace Slime.Configs.Skills
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 0)]
    public class SkillsConfig : ItemsConfig<SkillConfig, SkillData>
    {
        private const string ENTITY = "Skills";
    }
}