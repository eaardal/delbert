using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
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

            ResolveAllActorsOnce(container);
        }

        private static void ResolveAllActorsOnce(IContainer container)
        {
            //container.ResolveNamed<IActorRef>(ActorRegistry.Client.ServerConnector.Name);
            //container.ResolveNamed<IActorRef>(ActorRegistry.Client.Lobby.Name);
            //container.ResolveNamed<IActorRef>(ActorRegistry.Client.GameLobby.Name);
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
