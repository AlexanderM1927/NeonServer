using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Users.Inventory.Bots;

namespace Neon.Communication.Packets.Outgoing.Inventory.Bots
{
    class BotInventoryComposer : ServerPacket
    {
        public BotInventoryComposer(ICollection<Bot> Bots)
            : base(ServerPacketHeader.BotInventoryMessageComposer)
        {
            base.WriteInteger(Bots.Count);
            foreach (Bot Bot in Bots.ToList())
            {
                base.WriteInteger(Bot.Id);
               base.WriteString(Bot.Name);
               base.WriteString(Bot.Motto);
               base.WriteString(Bot.Gender);
               base.WriteString(Bot.Figure);
            }
        }
    }
}