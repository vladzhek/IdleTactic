using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.Data.IDs;
using Slime.Gameplay.Battle.Skills;

namespace Slime.Factories
{
    public class SkillFactory : ISkillFactory
    {
        private readonly IObjectFactory _objectFactory;

        public SkillFactory(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        public ISkill CreateSkill(string id)
        {
            ISkill skill = id switch
            {
                SkillIDs.LASER_1 => _objectFactory.CreateObject<LaserSkill>(),
                SkillIDs.LARGE_SHELL_1 => _objectFactory.CreateObject<AOEMissile>(),
                SkillIDs.ATTACK_SPEED_BOOST_1 => _objectFactory.CreateObject<AttackSpeed>(),
                SkillIDs.LASER_2 => _objectFactory.CreateObject<LaserSkill>(),
                SkillIDs.LARGE_SHELL_2 => _objectFactory.CreateObject<AOEMissile>(),
                SkillIDs.ATTACK_SPEED_BOOST_2 => _objectFactory.CreateObject<AttackSpeed>(),
                SkillIDs.LASER_3 => _objectFactory.CreateObject<LaserSkill>(),
                SkillIDs.LARGE_SHELL_3 => _objectFactory.CreateObject<AOEMissile>(),
                SkillIDs.ATTACK_SPEED_BOOST_3 => _objectFactory.CreateObject<AttackSpeed>(),
                SkillIDs.LASER_4 => _objectFactory.CreateObject<LaserSkill>(),
                SkillIDs.LARGE_SHELL_4 => _objectFactory.CreateObject<AOEMissile>(),
                SkillIDs.ATTACK_SPEED_BOOST_4 => _objectFactory.CreateObject<AttackSpeed>(),
                SkillIDs.LASER_5 => _objectFactory.CreateObject<LaserSkill>(),
                SkillIDs.LARGE_SHELL_5 => _objectFactory.CreateObject<AOEMissile>(),
                SkillIDs.ATTACK_SPEED_BOOST_5 => _objectFactory.CreateObject<AttackSpeed>(),
                _ => null
            };
            
            skill?.SetID(id);

            return skill;
        }
    }
}