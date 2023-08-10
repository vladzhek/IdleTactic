using Services;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.Data.IDs;
using Slime.Gameplay.Battle.Skills.Abstract;
using Slime.Services.EffectsService;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.Gameplay.Battle.Skills
{
    // NOTE: rename SingleTargetAttack
    public class LaserSkill : Skill
    {
        private const float COOLDOWN = 15f;

        private readonly IPlayer _player;
        private readonly AimService _aimService;
        private readonly IEffectsService _effectsService;
        private readonly ISkillsModel _skillsModel;

        public override string ID { get; set; }
        public override float SecondsToCooldown { get; set; }

        public LaserSkill(IPlayer player, AimService aimService, IEffectsService effectsService, ISkillsModel skillsModel)
        {
            _effectsService = effectsService;
            _player = player;
            _aimService = aimService;
            _skillsModel = skillsModel;
        }
        
        public override void Activate()
        {
            base.Activate();

            SecondsToCooldown = _skillsModel.Get(ID).Cooldown;
            var target = _aimService.GetClosestToPlayerEnemyUnit();
            var damage = _player.Unit.GetAttribute(AttributeIDs.Damage).FinalValue * _skillsModel.Get(ID).ActiveValue;

            if (target == null)
            {
                return;
            }

            var request = new EffectRequest(EffectIDs.LASER, target.Position, Quaternion.identity)
                .SetTarget(target.Avatar.ProjectileOrigin.position)
                .SetLifetime(1);
            _effectsService.RequestEffect(request).SetPromise(target.ApplyDamage(damage));
        }
    }
}