using System;
using System.Collections.Generic;

namespace Reactive
{
    public class ReactiveEnum<T> where T : Enum
    {
        public event Action<T> Changed;

        public ReactiveEnum(T value)
        {
            _value = value;
        }

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                {
                    return;
                }

                _value = value;
                Changed?.Invoke(_value);
            }
        }
    }
}