using Neon.Communication.Packets.Outgoing.Inventory.Bots;
using Neon.HabboHotel.Users.Inventory.Bots;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Inventory.Bots
{
    internal class GetBotInventoryEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().GetInventoryComponent() == null)
            {
                return;
            }

            ICollection<Bot> Bots = Session.GetHabbo().GetInventoryComponent().GetBots();
            Session.SendMessage(new BotInventoryComposer(Bots));
        }
    }
}
