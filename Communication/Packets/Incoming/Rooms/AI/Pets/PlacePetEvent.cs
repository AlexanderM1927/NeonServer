﻿using log4net;
using Neon.Communication.Packets.Outgoing.Inventory.Pets;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Rooms.AI;
using Neon.HabboHotel.Rooms.AI.Speech;
using System.Collections.Generic;

namespace Neon.Communication.Packets.Incoming.Rooms.AI.Pets
{
    internal class PlacePetEvent : IPacketEvent
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.Communication.Packets.Incoming.Rooms.AI.Pets.PlacePetEvent");

        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room Room))
            {
                return;
            }

            if ((Room.AllowPets == 0 && !Room.CheckRights(Session, true)) || !Room.CheckRights(Session, true))
            {
                Session.SendMessage(new RoomErrorNotifComposer(1));
                return;
            }

            if (Room.GetRoomUserManager().PetCount > NeonStaticGameSettings.RoomPetPlacementLimit)
            {
                Session.SendMessage(new RoomErrorNotifComposer(2));//5 = I have too many.
                return;
            }

            if (!Session.GetHabbo().GetInventoryComponent().TryGetPet(Packet.PopInt(), out Pet Pet))
            {
                return;
            }

            if (Pet == null)
            {
                return;
            }

            if (Pet.PlacedInRoom)
            {
                Session.SendNotification("Esta mascota ya se encuentra en la sala?");
                return;
            }

            int X = Packet.PopInt();
            int Y = Packet.PopInt();

            if (!Room.GetGameMap().CanWalk(X, Y, false))
            {
                Session.SendMessage(new RoomErrorNotifComposer(4));
                return;
            }

            if (Room.GetRoomUserManager().TryGetPet(Pet.PetId, out RoomUser OldPet))
            {
                Room.GetRoomUserManager().RemoveBot(OldPet.VirtualId, false);
            }

            Pet.X = X;
            Pet.Y = Y;

            Pet.PlacedInRoom = true;
            Pet.RoomId = Room.RoomId;

            List<RandomSpeech> RndSpeechList = new List<RandomSpeech>();
            RoomBot RoomBot = new RoomBot(Pet.PetId, Pet.RoomId, "pet", "freeroam", Pet.Name, "", Pet.Look, X, Y, 0, 0, 0, 0, 0, 0, ref RndSpeechList, "", 0, Pet.OwnerId, false, 0, false, 0);
            if (RoomBot == null)
            {
                return;
            }

            Room.GetRoomUserManager().DeployBot(RoomBot, Pet);

            Pet.DBState = DatabaseUpdateState.NeedsUpdate;
            Room.GetRoomUserManager().UpdatePets();

            if (!Session.GetHabbo().GetInventoryComponent().TryRemovePet(Pet.PetId, out Pet ToRemove))
            {
                log.Error("Error whilst removing pet: " + ToRemove.PetId);
                return;
            }

            Session.SendMessage(new PetInventoryComposer(Session.GetHabbo().GetInventoryComponent().GetPets()));
        }
    }
}
