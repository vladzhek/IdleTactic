using System;
using Slime.AbstractLayer.StateMachine;

namespace Slime.Gameplay.Behaviours.States
{
    [Serializable]
    public abstract class UnitBehaviourState : IUnitBehaviourState
    {
        public IUnitBehaviour UnitBehaviour { get; private set; }
        
        protected UnitBehaviourState(IUnitBehaviour unitBehaviour)
        {
            UnitBehaviour = unitBehaviour;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public abstract void OnUpdate(float delta);

        public void Dispose()
        {
            UnitBehaviour = null;
        }
    }
}