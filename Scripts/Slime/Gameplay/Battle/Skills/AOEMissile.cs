using ModestTree;
using Services;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.Data.IDs;
using Slime.Gameplay.Battle.Skills.Abstract;
using Slime.Services;
using Slime.Services.EffectsService;
using UnityEngine;
using Utils.Promises;
using Logger = Utils.Logger;

namespace Slime.Gameplay.Battle.Skills
{
    // NOTE: rename AOEAttack
    public class AOEMissile : Skill
    {
        private const float RADIUS = 5f;

        private readonly AimService _aimService;
        private readonly IPlayer _player;
        private readonly IEffectsService _effectsService;
        private readonly ISkillsModel _skillsModel;

        private readonly Transform _target;
        private readonly ICameraService _cameraService;

        public override string ID { get; set; }
        public override float SecondsToCooldown { get; set; }

        public AOEMissile(IPlayer player, AimService aimService, IEffectsService effectsService,
            ICameraService cameraService, ISkillsModel skillsModel)
        {
            _cameraService = cameraService;
            _effectsService = effectsService;
            _player = player;
            _aimService = aimService;
            _skillsModel = skillsModel;

            _target = new GameObject("TargetForAOESkill").transform;
        }

        public override void Activate()
        {
            base.Activate();

            SecondsToCooldown = _skillsModel.Get(ID).Cooldown;

            var target = _aimService.GetClosestToPlayerEnemyUnit();
            Vector3 aim = _player.Unit.Position + Vector3.right * 15f;
            if (target != null)
            {
                aim = target.Position;
            }

            var damage = _player.Unit.GetAttribute(AttributeIDs.Damage).FinalValue *
                         _skillsModel.Get(ID).ActiveValue;
            _target.position = aim;

            /*Logger.Log(
                $"final value: {_player.Unit.GetAttribute(AttributeIDs.Damage).FinalValue};"
                + $" skill active value: {_skillsModel.Get(ID).ActiveValue};" 
                + $" damage: {damage}");*/

            var attackPromise = new Promise(() =>
            {
                _cameraService.Shake();
                var targets = _aimService.GetUnitsInArea(aim, RADIUS);
                foreach (var unit in targets)
                {
                    unit.ApplyDamage(damage).Execute();
                }
            });

            _effectsService.RequestEffect(new EffectRequest(EffectIDs.AOE_MISSILE, aim + Vector3.up * 20f, Quaternion.identity)
                .SetLifetime(1)
                .SetDelay(1)
                .SetTarget(_target.position)).SetPromise(attackPromise);
        }
    }
}