using Akka.Actor;
using Akka.DI.AutoFac;
using Autofac;
using Autofac.Builder;

namespace Delbert.Infrastructure
{
    public static class AutofacExtensions
    {
        public static IRegistrationBuilder<IActorRef, SimpleActivatorData, SingleRegistrationStyle> 
            RegisterActor<TActor>(this ContainerBuilder builder, ActorSystem system, AutoFacDependencyResolver props, ActorMetadata actorEntry) where TActor : ActorBase
        {
            return builder.Register(ctx => system.ActorOf(props.Create<TActor>(), actorEntry.Name)).Named<IActorRef>(actorEntry.Name);
            //var actorInstance = system.ActorOf(props.Create<TActor>(), actorEntry.Name);
            //return builder.RegisterInstance(actorInstance).Named<IActorRef>(actorEntry.Name);
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> 
            WithInjectedActor<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder, ActorMetadata actorEntry) where TActivatorData : ReflectionActivatorData
        {
            return builder.WithParameter((param, ctx) => param.Name == actorEntry.Name, (param, ctx) => ctx.ResolveNamed<IActorRef>(actorEntry.Name));
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> 
            WithInjectedActorSelection<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder, ActorMetadata actorEntry, ActorPathType pathType) where TActivatorData : ReflectionActivatorData
        {
            return builder.WithParameter((param, ctx) => param.Name == actorEntry.Name, (param, ctx) =>
            {
                var actorSystem = ctx.Resolve<IActorSystemAdapter>();
                return actorSystem.ActorSelection(actorEntry.Path);
            });
        }
    }
}
