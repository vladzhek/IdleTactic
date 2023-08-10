﻿using Services;
using Slime.AbstractLayer.StateMachine;

namespace Slime.Gameplay.Behaviours.States.CompanionsStates
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
            var target = _aimService.GetRandomEnemyUnit(UnitBehaviour.Unit.Position);
            if (target != null)
            {
                UnitBehaviour.Unit.SetTarget(target);
            }

            UnitBehaviour.SetState(EBehaviourTrigger.FollowCharacter);
        }
    }
}