using Fleck;
using Neon.Communication.Packets.Incoming;
using Neon.Communication.Packets.Outgoing;
using System;

namespace Neon.HabboHotel.GameClients
{
    public class Session
    {
        public readonly IWebSocketConnection socket;
        public readonly Guid identifier;
        public GameClient client;

        public Session(IWebSocketConnection socket)
        {
            this.socket = socket;
            identifier = socket.ConnectionInfo.Id;
        }

        public void handleMessage(byte[] bytes)
        {
            try
            {
                ClientPacket packet = new ClientPacket(bytes);
                Console.WriteLine("SOCKET Packet:" + packet.Id);
                if (packet.Id == 1)
                {
                    int id = packet.PopInt();
                    string ssoTicket = packet.PopString();
                    Console.WriteLine(id + "  -  " + ssoTicket);

                    GameClient client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(id);

                    if (client == null || client.ssoTicket != ssoTicket)
                    {
                        Console.WriteLine("No coincide.");
                        Console.WriteLine(client.ssoTicket);
                        Console.WriteLine(ssoTicket);
                        socket.Close();
                        return;
                    }

                    client.wsSession = this;
                    this.client = client;

                    ServerPacket loginSso = new ServerPacket(1);
                    send(loginSso);

                }

                if (packet.Id == 2)
                {
                    bool test = packet.PopBoolean();

                    if (client == null)
                    {
                        socket.Close();
                        return;
                    }

                    Console.WriteLine(test);

                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw e;
            }
        }

        public void send(ServerPacket packet)
        {
            if (socket.IsAvailable)
            {
                socket.Send(packet.GetBytes());
            }
        }

        public void onEnd()
        {
            client.wsSession = null;
            client = null;
        }
    }
}
