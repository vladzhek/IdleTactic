using System;

namespace Reactive
{
    public class ReactiveProperty<T> where T : IEquatable<T>
    {
        public event Action<T> Changed;

        public ReactiveProperty(T value)
        {
            _value = value;
        }

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (_value != null && _value.Equals(value))
                {
                    return;
                }

                _value = value;
                Changed?.Invoke(_value);
            }
        }
    }
}