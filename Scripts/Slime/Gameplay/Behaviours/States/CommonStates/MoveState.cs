using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.IDs;
using UnityEngine;

namespace Slime.Gameplay.Behaviours.States.CommonStates
{
    public class MoveState : UnitBehaviourState
    {
        private IUnit Target => UnitBehaviour.Unit.Target;
        private float MoveSpeed => UnitBehaviour.Unit.GetAttribute(AttributeIDs.MoveSpeed)?.FinalValue ?? 0;
        private float AttackRadius => UnitBehaviour.Unit.GetAttribute(AttributeIDs.AttackRadius)?.FinalValue ?? 0;

        public MoveState(IUnitBehaviour unitBehaviour) : base(unitBehaviour)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            UnitBehaviour.Unit.Avatar.SetMoveState(true);
        }

        public override void OnExit()
        {
            base.OnExit();

            UnitBehaviour.Unit.Avatar.SetMoveState(false);
        }

        public override void OnUpdate(float delta)
        {
            if (UnitBehaviour.Unit.Avatar == null)
            {
                return;
            }

            if (Target?.Avatar == null)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.FindTarget);
                return;
            }

            var distance = UnitBehaviour.Unit.Position.x - Target.Position.x;
            if (Mathf.Abs(distance) < AttackRadius)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.Attack);
                return;
            }

            var moveVector = Vector3.left * (Mathf.Sign(distance) * MoveSpeed * delta);
            var rotation = Mathf.Sign(distance) > 0 ? Quaternion.LookRotation(Vector3.forward) : Quaternion.LookRotation(Vector3.back);
            UnitBehaviour.Unit.SetPosition(UnitBehaviour.Unit.Position + moveVector, rotation);
        }
    }
}