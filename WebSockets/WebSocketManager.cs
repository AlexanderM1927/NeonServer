using Fleck;

namespace Neon.WebSockets
{
    internal class WebSocketManager
    {
        public static void StartListener()
        {
            WebSocketServer server = new WebSocketServer("ws://0.0.0.0:8181");
            server.Start(socket =>
            {
                socket.OnOpen = () => NeonEnvironment.GetGame().GetClientManager().registerSession(socket);
                socket.OnClose = () => NeonEnvironment.GetGame().GetClientManager().closeSession(socket);
                socket.OnBinary = message =>
                {
                    NeonEnvironment.GetGame().GetClientManager().sessionHandleMessage(socket, message);
                };
            });
        }
    }
}
