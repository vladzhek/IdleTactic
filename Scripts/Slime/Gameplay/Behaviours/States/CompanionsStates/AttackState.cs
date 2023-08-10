using System.Collections.Generic;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.IDs;
using Slime.Services.EffectsService;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.Gameplay.Behaviours.States.CompanionsStates
{
    public class AttackState : UnitBehaviourState
    {
        private readonly IPlayer _player;
        private readonly ICompanionsModel _companionsModel;
        private readonly IEffectsService _effectsService;
        
        private float _timeSinceAttack;

        public AttackState(IUnitBehaviour unitBehaviour,
            IPlayer player,
            ICompanionsModel companionsModel,
            IEffectsService effectsService) : base(unitBehaviour)
        {
            _companionsModel = companionsModel;
            _player = player;
            _effectsService = effectsService;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            _timeSinceAttack = 0;
        }

        public override void OnUpdate(float delta)
        {
            if (!CanAttack)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.FollowCharacter);
                return;
            }

            _timeSinceAttack += delta;
            if (_timeSinceAttack > 1 / UnitBehaviour.Unit.GetAttribute(AttributeIDs.AttackSpeed).FinalValue)
            {
                Attack();
                _timeSinceAttack = 0;
            }
        }
        
        private void Attack()
        {
            var index = UnitBehaviour.Unit.Index;
            var id = _companionsModel.GetEquipData().GetValueOrDefault(index);
            if (id == null)
            {
                return;
            }

            var data = _companionsModel.Get(id);
            if (data == null)
            {
                return;
            }
            
            var damage = data.ActiveValue * _player.Unit.GetAttribute(AttributeIDs.Damage).FinalValue;
            // NOTE: no critical for companions
            /*var isCritical = Random.value < UnitBehaviour.Unit.GetAttribute(AttributeIDs.CriticalChance).FinalValue / 100;
            if (isCritical)
            {
                damage *= UnitBehaviour.Unit.GetAttribute(AttributeIDs.CriticalDamage).FinalValue / 100;
            }*/

            if (!CanAttack)
            {
                return;
            }
            var attack = Target.ApplyDamage(damage);
            
            var timeToTarget = GetTimeToTarget();
            var targetPosition = UnitBehaviour.Unit.Target.Avatar.ProjectileOrigin.position;
            var projectilePosition = UnitBehaviour.Unit.Avatar.ProjectileOrigin.position;
            var offsetDirection = (targetPosition - projectilePosition).normalized;
            const float offset = -10f;
            var effectPosition = projectilePosition + offsetDirection * offset;
            
            var request = new EffectRequest(EffectIDs.PLAYER_ATTACK, effectPosition, Quaternion.identity)
                .SetTarget(targetPosition)
                .SetLifetime(1)
                .SetDelay(timeToTarget);
            _effectsService.RequestEffect(request).SetPromise(attack);

            UnitBehaviour.Unit.Avatar.TriggerAction();
        }

        private float GetTimeToTarget()
        {
            if (!CanAttack)
            {
                return 0;
            }
            
            const float speed = 2f;
            var distance = Vector3.Distance(UnitBehaviour.Unit.Position, TargetPosition);
            return distance / speed * Time.deltaTime;
        }

        private IUnit Target => UnitBehaviour.Unit.Target;
        private Vector3 TargetPosition => Target?.Avatar?.ProjectileOrigin?.position ?? Vector3.zero;
        private bool CanAttack => (Target?.GetParameter(AttributeIDs.Health)?.PromisedValue ?? 0) > 0 
                                  && TargetPosition != Vector3.zero;
    }
}