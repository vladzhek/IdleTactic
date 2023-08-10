using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.Data.Boosters;
using Slime.Models;
using Utils.Time;

namespace Slime.Gameplay.Battle.Boosters
{
    public class SkillCooldownBlessingBooster : Booster
    {
        private readonly ISkillsManager _skillsManager;

        private Dictionary<string, float> _preBoostValue = new();

        public SkillCooldownBlessingBooster(BoosterProgressData progress, BoosterData data, ISkillsManager skillsManager,
            TimerService timerService) :
            base(progress, data, timerService)
        {
            _skillsManager = skillsManager;
        }

        public override void Activate()
        {
            base.Activate();

            foreach (var skill in 
                     from skill in _skillsManager.Skills.Values 
                     where skill != null 
                     select skill)
            {
                _preBoostValue[skill.ID] = skill.SecondsToCooldown;

                skill.SecondsToCooldown -= skill.SecondsToCooldown * Data.StartingValue;
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();

            foreach (var skill in _skillsManager.Skills.Values)
            {
                if (skill == null) continue;

                if (_preBoostValue.TryGetValue(skill.ID, out float value))
                {
                    skill.SecondsToCooldown = value;
                }
            }
        }
    }
}