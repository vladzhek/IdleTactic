using System.Collections.Generic;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.Data.IDs.Boosters;
using Slime.Services;
using Slime.Services.EffectsService;
using UnityEngine;
using Random = UnityEngine.Random;
using Logger = Utils.Logger;

namespace Slime.Gameplay.Behaviours.States.PlayerStates
{
    public class AttackState : UnitBehaviourState
    {
        private readonly ICameraService _cameraService;
        private readonly IEffectsService _effectsService;
        private readonly IParametersModel _parametersModel;
        private readonly IBoostersModel _boostersModel;
        
        private float _timeSinceAttack;

        public AttackState(
            IUnitBehaviour unitBehaviour, 
            ICameraService cameraService,
            IEffectsService effectsService, 
            IParametersModel parametersModel,
            IBoostersModel boostersModel
            ) : base(unitBehaviour)
        {
            _effectsService = effectsService;
            _cameraService = cameraService;
            _parametersModel = parametersModel;
            _boostersModel = boostersModel;
        }

        public override void OnUpdate(float delta)
        {
            if (UnitBehaviour.Unit.Target == null)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.FindTarget);
                return;
            }

            var targetHealth = UnitBehaviour.Unit.Target.GetParameter(AttributeIDs.Health);
            if (targetHealth == null || targetHealth.PromisedValue <= 0)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.FindTarget);
                return;
            }

            _timeSinceAttack += delta;

            var attackSpeed = _parametersModel.Get(EAttribute.AttackSpeed);
            if (_timeSinceAttack > 1 / attackSpeed)
            {
                var isTripleAttack = Random.value < _parametersModel.Get(EAttribute.TripleAttackChance);
                var isDoubleAttack = Random.value < _parametersModel.Get(EAttribute.DoubleAttackChance);

                if (isTripleAttack)
                {
                    Attack();
                    Attack();
                }
                else if (isDoubleAttack)
                {
                    Attack();
                }

                Attack();
            }
        }

        private void Attack()
        {
            var damage = _parametersModel.Get(EAttribute.Damage);

            // TODO: move this to parameters model
            var damageBlessing = _boostersModel.Boosters.GetValueOrDefault(BoostersIDs.ATK);
            var blessingValue = damageBlessing?.BlessingValue ?? 1;
            damage *= blessingValue;
            
            //Logger.Log($"damage: {damage}");

            var isCritical = Random.value < _parametersModel.Get(EAttribute.CriticalChance);
            if (isCritical)
            {
                _cameraService.Shake();
                damage *= 1 + _parametersModel.Get(EAttribute.CriticalDamage);
            }

            var attack = UnitBehaviour.Unit.Target.ApplyDamage(damage);
            
            var targetPosition = UnitBehaviour.Unit.Target.Avatar.ProjectileOrigin.position;
            var projectilePosition = UnitBehaviour.Unit.Avatar.ProjectileOrigin.position;
            var offsetDirection = (targetPosition - projectilePosition).normalized;
            const float offset = -8f;
            var effectPosition = projectilePosition + offsetDirection * offset;
            
            var request = new EffectRequest(EffectIDs.PLAYER_ATTACK, effectPosition, Quaternion.identity)
                .SetTarget(targetPosition)
                .SetLifetime(1)
                .SetDelay(GetTimeToTarget());
            _effectsService.RequestEffect(request).SetPromise(attack);

            UnitBehaviour.Unit.Avatar.TriggerAction();
            _timeSinceAttack = 0;
        }

        private float GetTimeToTarget()
        {
            const float speed = 2f;
            var distance = Vector3.Distance(UnitBehaviour.Unit.Position, UnitBehaviour.Unit.Target.Position);
            return distance / speed * Time.deltaTime;
        }
    }
}