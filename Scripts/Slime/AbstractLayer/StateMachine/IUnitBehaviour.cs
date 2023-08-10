using System;
using Slime.AbstractLayer.Battle;

namespace Slime.AbstractLayer.StateMachine
{
    public interface IUnitBehaviour : IDisposable, IUpdatable
    {
        IUnit Unit { get; }

        void SetUnit(IUnit unit);
        void SetState(EBehaviourTrigger trigger);
    }
}