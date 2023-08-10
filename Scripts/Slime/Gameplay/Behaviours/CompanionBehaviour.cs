using Slime.AbstractLayer;
using Slime.AbstractLayer.StateMachine;
using Slime.Behaviours;
using Slime.Gameplay.Behaviours.States.CompanionsStates;

namespace Slime.Gameplay.Behaviours
{
    public class CompanionBehaviour : UnitBehaviour
    {
        private IObjectFactory _objectFactory;
        
        private CompanionBehaviour(
            IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            var followCharacterState = _objectFactory.CreateObject<FollowCharacterState>(new object[] { this }); 
            var findTargetState = _objectFactory.CreateObject<FindTargetState>(new object[] { this }); 
            var attackState = _objectFactory.CreateObject<AttackState>(new object[] { this });
            
            ConfigureState(followCharacterState)
                .PermitReentry(EBehaviourTrigger.FollowCharacter)
                .Permit(EBehaviourTrigger.FindTarget, findTargetState)
                .Permit(EBehaviourTrigger.Attack, attackState);
            
            ConfigureState(findTargetState)
                .Permit(EBehaviourTrigger.FollowCharacter, followCharacterState);

            ConfigureState(attackState)
                .PermitReentry(EBehaviourTrigger.Attack)
                .Permit(EBehaviourTrigger.FollowCharacter, followCharacterState);

            SetState(followCharacterState);
            OnEnterState(CurrentState);
        }

        public override void Dispose()
        {
            base.Dispose();

            _objectFactory = null;
        }
    }
}