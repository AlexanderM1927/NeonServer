﻿using Neon.HabboHotel.Items;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.Furni.Moodlight
{
    internal class ToggleMoodlightEvent : IPacketEvent
    {
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

            if (!Room.CheckRights(Session, true) || Room.MoodlightData == null)
            {
                return;
            }

            Item Item = Room.GetRoomItemHandler().GetItem(Room.MoodlightData.ItemId);
            if (Item == null || Item.GetBaseItem().InteractionType != InteractionType.MOODLIGHT)
            {
                return;
            }

            if (Room.MoodlightData.Enabled)
            {
                Room.MoodlightData.Disable();
            }
            else
            {
                Room.MoodlightData.Enable();
            }

            Item.ExtraData = Room.MoodlightData.GenerateExtraData();
            Item.UpdateState();
        }
    }
}