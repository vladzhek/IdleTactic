using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.StateMachine;

namespace Slime.Behaviours
{
    public abstract class UnitBehaviour : StateMachineBase<IUnitBehaviourState, EBehaviourTrigger>, IUnitBehaviour
    {
        public IUnit Unit { get; private set; }
        
        public void SetUnit(IUnit unit)
        {
            Unit = unit;
            Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();
            
            Unit = null;
        }

        public override void OnEnterState(IUnitBehaviourState unitBehaviourState)
        {
            base.OnEnterState(unitBehaviourState);
            
            var state = unitBehaviourState.ToString();
            var startIndex = state.LastIndexOf('.');
            Unit.Status = state.Substring(startIndex, state.Length - startIndex);
        }

        void IUnitBehaviour.SetState(EBehaviourTrigger trigger)
        {
            FireTrigger(trigger);
        }
    }
}