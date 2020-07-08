using Neon.Communication.Packets.Outgoing.GameCenter;
using Neon.HabboHotel.Games;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.GameCenter
{
    internal class GetGameListingEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<GameData> Games = NeonEnvironment.GetGame().GetGameDataManager().GameData;

            Session.SendMessage(new GameListComposer(Games));
        }
    }
}
