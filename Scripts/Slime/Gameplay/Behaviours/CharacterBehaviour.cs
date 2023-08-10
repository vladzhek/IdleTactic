using Slime.AbstractLayer;
using Slime.AbstractLayer.StateMachine;
using Slime.Behaviours;
using Slime.Gameplay.Behaviours.States.CommonStates;
using Slime.Gameplay.Behaviours.States.EnemyStates;
using Zenject;
using AttackState = Slime.Gameplay.Behaviours.States.PlayerStates.AttackState;
using FindTargetState = Slime.Gameplay.Behaviours.States.PlayerStates.FindTargetState;

namespace Slime.Gameplay.Behaviours
{
    public class CharacterBehaviour : UnitBehaviour
    {
        private IObjectFactory _objectFactory;
        
        [Inject]
        private void Construct(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        public override void Initialize()
        {
            base.Initialize();

            var idleState = _objectFactory.CreateObject<IdleState>(new object[] { this });
            var attackState = _objectFactory.CreateObject<AttackState>(new object[] { this });
            var findTargetState = _objectFactory.CreateObject<FindTargetState>(new object[] { this });
            var moveState = _objectFactory.CreateObject<MoveState>(new object[] { this });

            ConfigureState(idleState)
                .Permit(EBehaviourTrigger.FindTarget, findTargetState);
            ConfigureState(attackState)
                .Permit(EBehaviourTrigger.Idle, idleState)
                .Permit(EBehaviourTrigger.FindTarget, findTargetState);
            ConfigureState(findTargetState)
                .Permit(EBehaviourTrigger.Move, moveState)
                .Permit(EBehaviourTrigger.Idle, idleState);
            ConfigureState(moveState)
                .Permit(EBehaviourTrigger.Attack, attackState)
                .Permit(EBehaviourTrigger.FindTarget, findTargetState);

            SetState(idleState);
            OnEnterState(CurrentState);
        }

        public override void Dispose()
        {
            base.Dispose();

            _objectFactory = null;
        }
    }
}