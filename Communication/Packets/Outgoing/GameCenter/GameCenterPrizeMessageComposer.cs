using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Games;
using Neon.HabboHotel.Users;

namespace Neon.Communication.Packets.Outgoing.GameCenter
{
    public class GameCenterPrizeMessageComposer : ServerPacket
    {
        public GameCenterPrizeMessageComposer(int GameId)
            : base(ServerPacketHeader.GameCenterPrizeMessageComposer)
        {
            base.WriteInteger(GameId);
            base.WriteInteger(1);
            base.WriteString("s");
            base.WriteInteger(230); // SpriteID
            base.WriteString("throne");
            base.WriteInteger(3);
            base.WriteBoolean(false);
            base.WriteInteger(10000);
            base.WriteBoolean(true);        
        }
    }
}
