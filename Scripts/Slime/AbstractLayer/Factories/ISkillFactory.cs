using Slime.AbstractLayer.Battle;

namespace Slime.Factories
{
    public interface ISkillFactory
    {
        ISkill CreateSkill(string id);
    }
}