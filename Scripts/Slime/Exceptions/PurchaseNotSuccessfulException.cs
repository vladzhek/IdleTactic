using System;

namespace Slime.Exceptions
{
    public class PurchaseNotSuccessfulException: Exception
    {
        public PurchaseNotSuccessfulException(string message = "purchase was not successful") : base(message)
        {
        }
    }
}