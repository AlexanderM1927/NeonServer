﻿
using Neon.HabboHotel.Users.Messenger;

namespace Neon.Communication.Packets.Incoming.Messenger
{
    internal class AcceptBuddyEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
            {
                return;
            }

            int Amount = Packet.PopInt();
            if (Amount > 50)
            {
                Amount = 50;
            }
            else if (Amount < 0)
            {
                return;
            }

            for (int i = 0; i < Amount; i++)
            {
                int RequestId = Packet.PopInt();

                if (!Session.GetHabbo().GetMessenger().TryGetRequest(RequestId, out MessengerRequest Request))
                {
                    continue;
                }

                if (Request.To != Session.GetHabbo().Id)
                {
                    return;
                }

                if (!Session.GetHabbo().GetMessenger().FriendshipExists(Request.To))
                {
                    Session.GetHabbo().GetMessenger().CreateFriendship(Request.From);
                }

                Session.GetHabbo().GetMessenger().HandleRequest(RequestId);
            }
        }
    }
}
