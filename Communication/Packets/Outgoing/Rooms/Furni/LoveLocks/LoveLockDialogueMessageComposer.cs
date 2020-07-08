﻿namespace Neon.Communication.Packets.Outgoing.Rooms.Furni.LoveLocks
{
    internal class LoveLockDialogueMessageComposer : ServerPacket
    {
        public LoveLockDialogueMessageComposer(int ItemId)
            : base(ServerPacketHeader.LoveLockDialogueMessageComposer)
        {
            base.WriteInteger(ItemId);
            base.WriteBoolean(true);
        }
    }
}
