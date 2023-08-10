using Slime.AbstractLayer.StateMachine;

namespace Slime.Gameplay.Behaviours.States.CommonStates
{
    internal class IdleState : UnitBehaviourState
    {
        private float _time;
        
        public IdleState(IUnitBehaviour unitBehaviour) : base(unitBehaviour)
        {
        }

        public override void OnEnter()
        { 
            base.OnEnter();
            
            UnitBehaviour.Unit.Avatar.SetMoveState(false);
            _time = 0;
        }

        public override void OnUpdate(float delta)
        {
            _time += delta;
            if (_time > 1)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.FindTarget);
            }
        }
    }
}