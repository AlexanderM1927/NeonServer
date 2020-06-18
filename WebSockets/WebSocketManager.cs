using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neon.WebSockets
{
    class WebSocketManager
    {
        public static void StartListener()
        {
            var server = new WebSocketServer("ws://0.0.0.0:8181");
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
