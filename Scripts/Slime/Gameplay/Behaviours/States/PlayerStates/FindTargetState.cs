using Services;
using Slime.AbstractLayer.StateMachine;

namespace Slime.Gameplay.Behaviours.States.PlayerStates
{
    public class FindTargetState : UnitBehaviourState
    {
        private AimService _aimService;

        public FindTargetState(IUnitBehaviour unitBehaviour, 
            AimService aimService) : base(unitBehaviour)
        {
            _aimService = aimService;
        }

        public override void OnUpdate(float delta)
        {
            var target = _aimService.GetClosestEnemyUnit(UnitBehaviour.Unit.Position);
            if (target == null)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.Idle);
                return;
            }

            UnitBehaviour.Unit.SetTarget(target);
            UnitBehaviour.SetState(EBehaviourTrigger.Move);
        }
    }
}