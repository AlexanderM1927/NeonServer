using Neon.Communication.Packets.Outgoing.Messenger;
using Neon.HabboHotel.Users.Messenger;
using System.Collections.Generic;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.Messenger
{
    internal class MessengerInitEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
            {
                return;
            }

            Session.GetHabbo().GetMessenger().OnStatusChanged(false);

            ICollection<MessengerBuddy> Friends = new List<MessengerBuddy>();
            foreach (MessengerBuddy Buddy in Session.GetHabbo().GetMessenger().GetFriends().ToList())
            {
                if (Buddy == null || Buddy.IsOnline)
                {
                    continue;
                }

                Friends.Add(Buddy);
            }

            Session.SendMessage(new MessengerInitComposer());
            Session.SendMessage(new BuddyListComposer(Friends, Session.GetHabbo()));

            try
            {
                if (Session.GetHabbo().GetMessenger() != null)
                {
                    Session.GetHabbo().GetMessenger().OnStatusChanged(true);
                }
            }
            catch { }

            Session.GetHabbo().GetMessenger().ProcessOfflineMessages();
        }
    }
}