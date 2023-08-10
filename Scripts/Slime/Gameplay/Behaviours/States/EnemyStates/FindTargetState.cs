using Slime.AbstractLayer;
using Slime.AbstractLayer.StateMachine;

namespace Slime.Gameplay.Behaviours.States.EnemyStates
{
    public class FindTargetState : UnitBehaviourState
    {
        private IPlayer _player;

        public FindTargetState(IUnitBehaviour unitBehaviour, IPlayer player) : base(unitBehaviour)
        {
            _player = player;
        }
        
        public override void OnUpdate(float delta)
        {
            if (_player.Unit is not { IsActive: true })
            {
                return;
            }

            UnitBehaviour.Unit.SetTarget(_player.Unit);
            UnitBehaviour.SetState(EBehaviourTrigger.Move);
        }
    }
}