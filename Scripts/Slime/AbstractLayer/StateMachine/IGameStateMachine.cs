using Slime.Data.Triggers;

namespace Slime.AbstractLayer.StateMachine
{
    public interface IGameStateMachine : IStateMachine<IState, EGameTriggers>
    {
    }
}