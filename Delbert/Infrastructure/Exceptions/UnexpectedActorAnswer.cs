using System;

namespace Delbert.Infrastructure.Exceptions
{
    public class UnexpectedActorAnswer : Exception
    {
        public UnexpectedActorAnswer(string message) : base(message)
        {
            
        }
    }
}