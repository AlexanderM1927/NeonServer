﻿using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Communication.Packets.Outgoing.Rooms.Session;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Misc
{
    internal class EventTrackerEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            _ = Packet.PopString();
            string Test2 = Packet.PopString();
            string Test3 = Packet.PopString();
            _ = Packet.PopString();
            _ = Packet.PopInt();

            if (Session.GetHabbo().MultiWhisper && Test3.Equals("RWUAM_WHISPER_USER"))
            {
                Room Room = Session.GetHabbo().CurrentRoom;
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().LastUserId);
                if (!Session.GetHabbo().MultiWhispers.Contains(User))
                {
                    Session.GetHabbo().MultiWhispers.Add(User);
                    Session.SendMessage(RoomNotificationComposer.SendBubble("multi_whisper", "Has añadido a esta persona a tu lista de susurros múltiples.", ""));
                    return;
                }
                else
                {
                    Session.GetHabbo().MultiWhispers.Remove(User);
                }

                Session.SendMessage(RoomNotificationComposer.SendBubble("multi_whisper", "Has quitado a esta persona a tu lista de susurros múltiples.", ""));

                return;
            }

            if (Test3.Equals("showGameCenter.gameEnd") && Test2.Equals("battleball"))
            {
                Session.SendMessage(new RoomForwardComposer(2));
            }

            //if (Test3.Equals("RWUAM_START_TRADING") && Session.GetHabbo().SecureTradeEnabled == false && Session.GetHabbo().SecurityQuestion == false)
            //{
            //    Session.SendMessage(new WiredSmartAlertComposer("Tienes desactivado el modo seguro de tradeo de dados. ¿Quieres activarlo? Di: Sí."));
            //Session.GetHabbo().IsBeingAsked = true;
            //Room Room = Session.GetHabbo().CurrentRoom;

            //if(Room.GetRoomItemHandler().GetFloor.ToList().Contains;
            //}
            //Session.SendWhisper(Test1 + " - " + Test2 + " - " + Test3 + " - " + Test4 + " - " + Test5 + " - ");

            //Session.SendMessage(RoomNotificationComposer.SendBubble("multi_whisper", Test1 + " - " + Test2 + " - " + Test3 + " - " + Test4 + " - " + Test5 + " - ", ""));
        }
    }
}
