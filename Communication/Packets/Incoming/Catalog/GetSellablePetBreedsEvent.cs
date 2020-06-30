using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.HabboHotel.GameClients;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    public class GetSellablePetBreedsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();
            int PetId = NeonEnvironment.GetGame().GetCatalog().GetPetRaceManager().GetPetId(Type, out string PacketType);

            Session.SendMessage(new SellablePetBreedsComposer(PacketType, PetId, NeonEnvironment.GetGame().GetCatalog().GetPetRaceManager().GetRacesForRaceId(PetId)));
        }
    }
}