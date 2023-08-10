namespace Slime.AbstractLayer.StateMachine
{
    public abstract class State<T> : IState
    {
        protected ITriggerResponder<T> StateMachine { get; private set; }

        protected State(ITriggerResponder<T> stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Dispose()
        {
            StateMachine = default;
        }

        public abstract void OnEnter();

        public abstract void OnExit();
    }
}