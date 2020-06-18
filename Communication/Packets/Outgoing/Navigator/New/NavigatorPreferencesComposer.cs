namespace Neon.Communication.Packets.Outgoing.Navigator
{
    class NavigatorPreferencesComposer : ServerPacket
    {
        public NavigatorPreferencesComposer()
            : base(ServerPacketHeader.NavigatorPreferencesMessageComposer)
        {
            base.WriteInteger(10);
            base.WriteInteger(17);
            base.WriteInteger(425);
            base.WriteInteger(591);
            base.WriteBoolean(false);
            base.WriteInteger(0);
        }
    }
}