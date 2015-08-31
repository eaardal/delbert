namespace Delbert.Infrastructure
{
    public class ActorRegistry
    {
        public const string ClientBaseUrl = "akka.tcp://coup-client@localhost/user/";
        public const string ServerBaseUrl = "akka.tcp://coup-server@localhost:8081/user/";

        public class Server
        {
            public static ActorEntry LoginController => new ActorEntry("loginController").WithNameAsUrl(ServerBaseUrl);
            public static ActorEntry GameLobbyManager => new ActorEntry("gameLobbyManager").WithNameAsUrl(ServerBaseUrl);
            public static ActorEntry GameLobbyController => new ActorEntry("gameLobbyController").WithNameAsUrl(ServerBaseUrl);
            public static ActorEntry GameSessionManager => new ActorEntry("gameSessionManager").WithNameAsUrl(ServerBaseUrl);
            public static ActorEntry GameSession => new ActorEntry("gameSession").WithNameAsUrl(ServerBaseUrl);
        }

        public class Client
        {
            public static ActorEntry ServerConnector => new ActorEntry("serverConnector").WithNameAsUrl(ClientBaseUrl);
            public static ActorEntry Lobby => new ActorEntry("lobby").WithNameAsUrl(ClientBaseUrl);
            public static ActorEntry GameLobby => new ActorEntry("gameLobby").WithNameAsUrl(ClientBaseUrl);
        }
    }
}
