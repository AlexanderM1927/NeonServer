
using Neon.Communication.Packets.Outgoing.Rooms.AI.Pets;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.AI.Pets
{
    internal class GetPetInformationEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            int PetId = Packet.PopInt();

            if (!Session.GetHabbo().CurrentRoom.GetRoomUserManager().TryGetPet(PetId, out RoomUser Pet))
            {
                //Okay so, we've established we have no pets in this room by this virtual Id, let us check out users, maybe they're creeping as a pet?!
                RoomUser User = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(PetId);
                if (User == null)
                {
                    return;
                }

                //Check some values first, please!
                if (User.GetClient() == null || User.GetClient().GetHabbo() == null)
                {
                    return;
                }

                //And boom! Let us send the information composer 8-).
                Session.SendMessage(new PetInformationComposer(User.GetClient().GetHabbo()));
                return;
            }

            //Continue as a regular pet..
            if (Pet.RoomId != Session.GetHabbo().CurrentRoomId || Pet.PetData == null)
            {
                return;
            }

            Session.SendMessage(new PetInformationComposer(Pet.PetData));
        }
    }
}
