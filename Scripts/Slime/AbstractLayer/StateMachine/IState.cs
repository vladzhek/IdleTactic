using System;

namespace Slime.AbstractLayer.StateMachine
{
    public interface IState : IDisposable
    {
        public void OnEnter();
        public void OnExit();
    }
}