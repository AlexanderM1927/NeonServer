using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Users;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Groups;
using Neon.Communication.Packets.Outgoing.Groups;
using Neon.Communication.Packets.Outgoing.Rooms.Permissions;
using Neon.HabboHotel.Cache;



namespace Neon.Communication.Packets.Incoming.Groups
{
    class TakeAdminRightsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!NeonEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (!Group.IsMember(UserId) || !Group.IsAdmin(Session.GetHabbo().Id))
            return;

            Habbo Habbo = NeonEnvironment.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("Oops, ocurrio un error mientras se realizaba la busqueda de este usuario.");
                return;
            }

            Group.TakeAdmin(UserId);

            Room Room = null;
            if (NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Group.RoomId, out Room))
            {
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
                if (User != null)
                {
                    if (User.Statusses.ContainsKey("flatctrl 3"))
                        User.RemoveStatus("flatctrl 3");
                    User.UpdateNeeded = true;
                    if (User.GetClient() != null)
                        User.GetClient().SendMessage(new YouAreControllerComposer(0));
                }
            }

            Session.SendMessage(new GroupMemberUpdatedComposer(GroupId, Habbo, 2));
        }
    }
}
