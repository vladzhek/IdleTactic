using Stateless;

namespace Slime.AbstractLayer.StateMachine
{
    public interface IStateMachine<TState, TTrigger> : 
        IUpdatable, 
        ITriggerResponder<TTrigger> 
        where TState : IState 
    {
        protected StateMachine<TState, TTrigger> StateMachine { get; }
        void Initialize();
        public TState GetState();
        void SetState(TState state);
        StateMachine<TState, TTrigger>.StateConfiguration ConfigureState(TState state);
        void OnEnterState(TState state);
        void OnExitState(TState state);
    }
}