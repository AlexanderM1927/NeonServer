
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Rooms;

namespace Neon.Communication.Packets.Incoming.Rooms.Engine
{
    internal class MoveAvatarEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null || !User.CanWalk)
            {
                return;
            }

            int MoveX = Packet.PopInt();
            int MoveY = Packet.PopInt();

            if (Session.GetHabbo().isDeveloping || Session.GetHabbo().PlayingChess)
            {
                Session.GetHabbo().lastX = MoveX;
                Session.GetHabbo().lastY = MoveY;
            }

            // NEON CUSTOM -REMOVE IF LAG
            if (!Room.GetGameMap().SquareIsOpen(MoveX, MoveY, false))
            { return; }

            if (MoveX == User.X && MoveY == User.Y)
            {
                User.SeatCount++;

                if (User.SeatCount == 4)
                {
                    User.SeatCount = 0;
                    return;
                }
            }

            else { User.SeatCount = 0; }

            if (User.RidingHorse)
            {
                RoomUser Horse = Room.GetRoomUserManager().GetRoomUserByVirtualId(User.HorseID);
                if (Horse != null)
                {
                    Horse.MoveTo(MoveX, MoveY);
                }
            }

            //int bubble = Session.GetHabbo().lastX++;

            //Session.SendChat("Burbuja número " + bubble, bubble);

            //if (Session.GetHabbo()._NUX)
            //{
            //    var nuxStatus = new ServerPacket(ServerPacketHeader.NuxUserStatus);
            //    nuxStatus.WriteInteger(2);
            //    Session.SendMessage(nuxStatus);
            //    Session.SendMessage(new NuxAlertComposer("nux/lobbyoffer/hide"));
            //    //Session.SendMessage(new NuxAlertComposer("helpBubble/add/HC_JOIN_BUTTON/Aquí puedes observar que dispones de HC Infinito."));
            //}

            if (Session.GetHabbo().isPasting)
            {
                Session.GetHabbo().lastX2 = MoveX;
                Session.GetHabbo().lastY2 = MoveY;
            }

            if (Session.GetHabbo().IsControlling)
            {
                RoomUser Controlled = Room.GetRoomUserManager().GetRoomUserByUsername(Session.GetHabbo().Opponent);
                if (Controlled != null || Controlled.CanWalk)
                {
                    Controlled.MoveTo(MoveX, MoveY);
                    return;
                }
                else
                {
                    Session.SendWhisper("El usuario al que controlas no existe o no puede moverse.", 34);
                }
            }

            User.MoveTo(MoveX, MoveY);
        }
    }
}