using System;

namespace Slime.Exceptions
{
    public class ProgressSaveException : Exception
    {
        public ProgressSaveException(string message = Slime.Data.Constants.Exceptions.PROGRESS_SAVE_DEFAULT) : base(message)
        {
            
        }
    }
}