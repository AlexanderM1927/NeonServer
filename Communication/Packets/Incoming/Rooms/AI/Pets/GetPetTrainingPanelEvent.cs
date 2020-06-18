﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Rooms.AI;
using Neon.HabboHotel.Rooms;
using Neon.Communication.Packets.Outgoing.Rooms.AI.Pets;

namespace Neon.Communication.Packets.Incoming.Rooms.AI.Pets
{
    class GetPetTrainingPanelEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            int PetId = Packet.PopInt();

            RoomUser Pet = null;
            if (!Session.GetHabbo().CurrentRoom.GetRoomUserManager().TryGetPet(PetId, out Pet))
            {
                //Okay so, we've established we have no pets in this room by this virtual Id, let us check out users, maybe they're creeping as a pet?!
                RoomUser User = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(PetId);
                if (User == null)
                    return;

                //Check some values first, please!
                if (User.GetClient() == null || User.GetClient().GetHabbo() == null)
                    return;

                //And boom! Let us send the training panel composer 8-).
                Session.SendWhisper("Tal vez un dia, boo boo", 34);
                return;
            }

            //Continue as a regular pet..
            if (Pet.RoomId != Session.GetHabbo().CurrentRoomId || Pet.PetData == null)
                return;

            Pet pet = Pet.PetData;
            Session.SendMessage(new PetTrainingPanelComposer(pet));
        }
    }
}
