using System;
using Utils;

namespace Slime.AbstractLayer.StateMachine
{
    public class StateMachineBase<TState, TTrigger> : 
        IStateMachine<TState, TTrigger> 
        where TState : IState, IDisposable
    {
        private Stateless.StateMachine<TState, TTrigger> _stateMachine;
        Stateless.StateMachine<TState, TTrigger> IStateMachine<TState, TTrigger>.StateMachine => _stateMachine;

        protected TState CurrentState;
        protected IUpdatable CurrentUpdatableState;

        public virtual void Initialize()
        {
            _stateMachine = new Stateless.StateMachine<TState, TTrigger>(GetState, SetState);
        }

        public TState GetState()
        {
            return CurrentState;
        }

        public void SetState(TState state)
        {
            //LogsWrap.Log($"New state {state}");
            CurrentState = state;
            if (state is IUpdatable updatable)
            {
                CurrentUpdatableState = updatable;
            }
            else
            {
                CurrentUpdatableState = null;
            }
        }

        public void FireTrigger(TTrigger trigger)
        {
            //Logger.Log($"trigger {trigger}");
            
            if (_stateMachine.CanFire(trigger))
            {
                try
                {
                    _stateMachine.Fire(trigger);
                }
                catch (Exception e)
                {
                    Logger.Error($"{trigger} state: {e}");
                }
            }
            else
            {
                Logger.Warning($"cant change {CurrentState} by trigger {trigger}");
            }
        }

        public virtual void Dispose()
        {
            _stateMachine.Deactivate();
            CurrentState?.Dispose();
            CurrentState = default;
        }

        public Stateless.StateMachine<TState, TTrigger>.StateConfiguration ConfigureState(TState state)
        {
            return _stateMachine.Configure(state).OnEntry(() => OnEnterState(state)).OnExit(() => OnExitState(state));
        }

        public virtual void OnEnterState(TState unitBehaviourState)
        {
            unitBehaviourState.OnEnter();
        }

        public virtual void OnExitState(TState unitBehaviourState)
        {
            unitBehaviourState.OnExit();
        }

        void IUpdatable.OnUpdate(float deltaTime)
        {
            CurrentUpdatableState?.OnUpdate(deltaTime);
        }
    }
}