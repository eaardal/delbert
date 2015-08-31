using System;

namespace Delbert.Infrastructure.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(object obj)
            : base("Invalid parameters given to " + obj.GetType().FullName)
        {
            
        }
    }
}