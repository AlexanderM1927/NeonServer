using Neon.Communication.Packets.Outgoing.Groups;
using Neon.Communication.Packets.Outgoing.Messenger;
using Neon.HabboHotel.Groups;
using Neon.HabboHotel.Users;

namespace Neon.Communication.Packets.Incoming.Groups
{
    internal class AcceptGroupMembershipEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            if (!NeonEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group Group))
            {
                return;
            }

            if ((Session.GetHabbo().Id != Group.CreatorId && !Group.IsAdmin(Session.GetHabbo().Id)) && !Session.GetHabbo().GetPermissions().HasRight("fuse_group_accept_any"))
            {
                return;
            }

            if (!Group.HasRequest(UserId))
            {
                return;
            }

            Habbo Habbo = NeonEnvironment.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("Oops, ha recibido un error mientras recibe la busqueda de este usuario.");
                return;
            }

            Group.HandleRequest(UserId, true);

            if (Group.HasChat)
            {
                HabboHotel.GameClients.GameClient Client = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                if (Client != null)
                {
                    Client.SendMessage(new FriendListUpdateComposer(Group, 1));
                }
            }

            Session.SendMessage(new GroupMemberUpdatedComposer(GroupId, Habbo, 4));
        }
    }
}