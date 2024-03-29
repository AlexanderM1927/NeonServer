﻿namespace Neon.Communication.Packets.Outgoing.GameCenter
{
    internal class PlayableGamesComposer : ServerPacket
    {
        public PlayableGamesComposer(int GameID)
            : base(ServerPacketHeader.PlayableGamesMessageComposer)
        {
            base.WriteInteger(GameID);
            base.WriteInteger(0);
        }
    }
}
