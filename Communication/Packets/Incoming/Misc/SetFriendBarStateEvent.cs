﻿
using Neon.Communication.Packets.Outgoing.Sound;
using Neon.HabboHotel.Users.Messenger.FriendBar;

namespace Neon.Communication.Packets.Incoming.Misc
{
    internal class SetFriendBarStateEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            Session.GetHabbo().FriendbarState = FriendBarStateUtility.GetEnum(1);
            Session.SendMessage(new SoundSettingsComposer(Session.GetHabbo().ClientVolume, Session.GetHabbo().ChatPreference, Session.GetHabbo().AllowMessengerInvites, Session.GetHabbo().FocusPreference, FriendBarStateUtility.GetInt(Session.GetHabbo().FriendbarState)));
        }
    }
}
