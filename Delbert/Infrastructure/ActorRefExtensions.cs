using System;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Infrastructure.Exceptions;
using Delbert.Infrastructure.Logging;

namespace Delbert.Infrastructure
{
    public class ActorResultBuilder
    {
        private readonly object _answer;
     
        public ActorResultBuilder(object answer)
        {
            if (answer == null) throw new ArgumentNullException(nameof(answer));
            _answer = answer;
        }

        public ActorResultBuilder WhenResultIs<T>(Action<T> action) where T : class
        {
            if (_answer is T)
            {
                action(_answer as T);
            }
            return this;
        }

        public ActorResultBuilder WhenResultIs<T1, T2>(Func<T1,T2> action) where T1 : class
        {
            if (_answer is T1)
            {
                action(_answer as T1);
            }
            return this;
        }
        
        public ActorResultBuilder LogFailure()
        {
            return LogFailure(null, null);
        }
        public ActorResultBuilder LogFailure(string message, params object[] args)
        {
            if (_answer is Failure)
            {
                var failure = _answer as Failure;

                if (string.IsNullOrEmpty(message) && !args.Any())
                {
                    Log.Msg(this, l => l.Error(failure.Exception));
                }
                Log.Msg(this, l => l.Error(string.Format(message, args), failure.Exception));
            }
            return this;
        }
    }
    
    public static class ActorRefExtensions
    {
        public static async Task<ActorResultBuilder> Query(this IActorRef actor, object message)
        {
            var answer = await actor.Ask(message);

            return new ActorResultBuilder(answer);
        }

        public static async Task<ActorResultBuilder> Query(this ActorSelection actor, object message)
        {
            var answer = await actor.Ask(message);

            return new ActorResultBuilder(answer);
        }

        //public static async Task<T> AskWithResultOf<T>(this IActorRef actor, object message) where T : class
        //{
        //    var answer = await actor.Ask(message);
        //    return ValidateAnswer<T>(answer);
        //}

        //public static async Task<T> AskWithResultOf<T>(this ActorSelection actor, object message) where T : class
        //{
        //    var answer = await actor.Ask(message);
        //    return ValidateAnswer<T>(answer);
        //}

        private static T ValidateAnswer<T>(object answer) where T : class
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