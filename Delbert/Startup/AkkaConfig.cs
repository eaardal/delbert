using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using Delbert.Actors;
using Delbert.Infrastructure;

namespace Delbert.Startup
{
    public class AkkaConfig
    {
        public static void Configure(IContainer container)
        {
            var system = CreateActorSystem();

            CreateAndRegisterActors(container, system);
        }

        private static void CreateAndRegisterActors(IContainer container, ActorSystem system)
        {
            var propsResolver = new AutoFacDependencyResolver(container, system);
            system.AddDependencyResolver(propsResolver);

            var builder = new ContainerBuilder();

            var actorSystemAdapter = container.Resolve<IActorSystemAdapter>(new TypedParameter(typeof(ActorSystem), system));
            builder.RegisterInstance(actorSystemAdapter).AsImplementedInterfaces().SingleInstance();
            builder.RegisterInstance(system).AsSelf().SingleInstance();

            //builder.RegisterActor<ServerConnector>(system, propsResolver, ActorRegistry.Client.ServerConnector);

            builder.Update(container);

            ResolveSingletonActors(actorSystemAdapter);
        }

        /// <summary>
        /// Resolves all singleton actors so that the instance has been created and does not throw exception when trying to retrieve it using ActorSelection later on.
        /// This will also trigger the actor's message subscriptions (in constructors) and do standard actor startup such as PreStart().
        /// </summary>
        /// <param name="actorSystem">The ActorSystem</param>
        private static void ResolveSingletonActors(IActorSystemAdapter actorSystem)
        {
            actorSystem.ActorOf(ActorRegistry.RootDirectory);
        }

        private static ActorSystem CreateActorSystem()
        {
            var config = GetConfig();

            return ActorSystem.Create("delbert", config);
        }

        private static Config GetConfig()
        {
            return ConfigurationFactory.Empty;
        }
    }
}
