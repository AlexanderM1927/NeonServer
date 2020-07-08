using Neon.Communication.Packets.Outgoing.Inventory.Pets;
using Neon.HabboHotel.Rooms.AI;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Inventory.Pets
{
    internal class GetPetInventoryEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().GetInventoryComponent() == null)
            {
                return;
            }

            ICollection<Pet> Pets = Session.GetHabbo().GetInventoryComponent().GetPets();
            Session.SendMessage(new PetInventoryComposer(Pets));
        }
    }
}