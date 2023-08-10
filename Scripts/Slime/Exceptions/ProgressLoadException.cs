using System;

namespace Slime.Exceptions
{
    public class ProgressLoadException : Exception
    {
        public ProgressLoadException(string message = Slime.Data.Constants.Exceptions.PROGRESS_LOAD_DEFAULT) : base(message)
        {
            
        }
    }
}