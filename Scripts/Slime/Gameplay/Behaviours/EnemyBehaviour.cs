using Services;
using Slime.AbstractLayer;
using Slime.AbstractLayer.StateMachine;
using Slime.Gameplay.Behaviours.States.CommonStates;
using Slime.Gameplay.Behaviours.States.EnemyStates;
using Zenject;

namespace Slime.Behaviours
{
    public class EnemyBehaviour : UnitBehaviour
    {
        private IdleState _idleState;
        private MoveState _moveState;
        private AttackState _attackState;
        private FindTargetState _findTarget;
        private IObjectFactory _objectFactory;

        [Inject]
        private void Construct(UpdateLoopsService updateLoopsService, IObjectFactory zenjectFactory)
        {
            _objectFactory = zenjectFactory;
        }

        public override void Initialize()
        {
            base.Initialize();

            _idleState = _objectFactory.CreateObject<IdleState>(new object[] { this });
            _moveState = _objectFactory.CreateObject<MoveState>(new object[] { this });
            _attackState = _objectFactory.CreateObject<AttackState>(new object[] { this });
            _findTarget = _objectFactory.CreateObject<FindTargetState>(new object[] { this });

            ConfigureState(_idleState)
                .Permit(EBehaviourTrigger.FindTarget, _findTarget);
            ConfigureState(_moveState)
                .Permit(EBehaviourTrigger.Attack, _attackState)
                .Permit(EBehaviourTrigger.FindTarget, _findTarget);
            ConfigureState(_attackState)
                .Permit(EBehaviourTrigger.Idle, _idleState)
                .Permit(EBehaviourTrigger.FindTarget, _findTarget);
            ConfigureState(_findTarget)
                .Permit(EBehaviourTrigger.Move, _moveState)
                .Permit(EBehaviourTrigger.Idle, _idleState);

            SetState(_idleState);
            OnEnterState(CurrentState);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _objectFactory = null;
            
            _idleState = null;
            _moveState = null;
            _attackState = null;
            _findTarget = null;
        }
    }
}