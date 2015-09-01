using System;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Infrastructure.Exceptions;
using Delbert.Infrastructure.Logging;

namespace Delbert.Infrastructure
{
    public static class ActorRefExtensions
    {
        public static async Task<T> AskWithResultOf<T>(this IActorRef actor, object message) where T : class
        {
            var answer = await actor.Ask(message);
            return ValidateAnswer<T>(answer);
        }

        public static async Task<T> AskWithResultOf<T>(this ActorSelection actor, object message) where T : class
        {
            var answer = await actor.Ask(message);
            return ValidateAnswer<T>(answer);
        }
        
        private static T ValidateAnswer<T>(object answer) where T:class
        {
            try
            {
                if (answer is T)
                {
                    return answer as T;
                }

                if (answer is Failure)
                {
                    var failure = answer as Failure;
                    Log.Msg(typeof(ActorRefExtensions), l => l.Error(failure.Exception));
                }

                throw new UnexpectedActorAnswer($"Expected {typeof(T).FullName}, Got {answer.GetType()}");
            }
            catch (Exception ex)
            {
                Log.Msg(typeof(ActorRefExtensions), l => l.Error(ex));
                throw;
            }
        }
    }
}