using System;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.IDs;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.Gameplay.Behaviours.States.CompanionsStates
{
    public class FollowCharacterState : UnitBehaviourState 
    {
        private readonly IPlayer _player;
        private readonly ISceneModel _sceneModel;
        
        private float _timeSinceAttack;
        private float _distanceFromPlayer;
        private Vector3 _direction;
        private Vector3 _velocity = Vector3.zero;
        
        public FollowCharacterState(IUnitBehaviour unitBehaviour, 
            IPlayer player,
            ISceneModel sceneModel) : base(unitBehaviour)
        {
            _player = player;
            _sceneModel = sceneModel;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            var index = UnitBehaviour.Unit.Index;
            var companionPosition = _sceneModel.CompanionsPositions[index].position.x;
            var characterPosition = _sceneModel.PlayerPosition.position.x;
            _direction = companionPosition > characterPosition ? Vector3.right : Vector3.left;
            _distanceFromPlayer = Mathf.Abs(companionPosition - characterPosition);
            
            UnitBehaviour.Unit.Avatar.SetMoveState(true);
        }

        public override void OnExit()
        {
            base.OnExit();

            UnitBehaviour.Unit.Avatar.SetMoveState(false);
        }

        public override void OnUpdate(float delta)
        {
            if (!CanMove)
            {
                return;
            }
            
            if (!HasTarget)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.FindTarget);
                return;
            }

            if (IsInPosition && CanAttack)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.Attack);
                return;
            }

            var targetPosition = CurrentPosition;
            targetPosition.x = CharacterPosition.x + _distanceFromPlayer * _direction.x;
            
            var position = Vector3.SmoothDamp(CurrentPosition, targetPosition, ref _velocity, .05f);
            UnitBehaviour.Unit.SetPosition(position);
        }

        private bool CanMove => UnitBehaviour.Unit.Avatar != null;
        private IUnit Target => UnitBehaviour.Unit.Target;
        private bool HasTarget => Target?.Avatar != null;
        private float AttackRadius => UnitBehaviour.Unit.GetAttribute(AttributeIDs.AttackRadius)?.FinalValue ?? 0;
        private bool CanAttack => Mathf.Abs(UnitBehaviour.Unit.Position.x - Target.Position.x) < AttackRadius;
        private Vector3 CharacterPosition => _player?.Unit?.Position ?? Vector3.zero;
        private Vector3 CurrentPosition => UnitBehaviour.Unit.Position;
        private bool IsInPosition => Math.Abs(CurrentPosition.x - (CharacterPosition.x + _distanceFromPlayer * _direction.x)) < 0.01f;
    }
}