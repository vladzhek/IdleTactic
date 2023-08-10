using System;

namespace Slime.Exceptions
{
    public class RewardNotSuccessfulException: Exception
    {
        public RewardNotSuccessfulException(string message = "reward was not successful") : base(message)
        {
        }
    }
}