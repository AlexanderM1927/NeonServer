using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms.AI;
using Neon.Communication.Packets.Incoming;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    public class GetSellablePetBreedsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();
            string PacketType = "";
            int PetId = NeonEnvironment.GetGame().GetCatalog().GetPetRaceManager().GetPetId(Type, out PacketType);

            Session.SendMessage(new SellablePetBreedsComposer(PacketType, PetId, NeonEnvironment.GetGame().GetCatalog().GetPetRaceManager().GetRacesForRaceId(PetId)));
        }
    }
}