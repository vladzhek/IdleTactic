using System;

namespace Utils.Promises
{
    public class Promise
    {
        private readonly Action _action;

        public Promise(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action?.Invoke();
        }
    }
}