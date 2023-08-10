using System;

namespace Slime.Exceptions
{
    public class NotEnoughResourceException : Exception
    {
        public NotEnoughResourceException(string message = "not enough resource") : base(message)
        {
        }
    }
}