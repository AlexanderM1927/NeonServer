using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.Games;

namespace Neon.Communication.Packets.Outgoing.GameCenter
{
    class GameListComposer : ServerPacket
    {
        public GameListComposer(ICollection<GameData> Games)
            : base(ServerPacketHeader.GameListMessageComposer)
        {
            base.WriteInteger(NeonEnvironment.GetGame().GetGameDataManager().GetCount());//Game count
            foreach (GameData Game in Games)
            {
               base.WriteInteger(Game.GameId);
               base.WriteString(Game.GameName);
               base.WriteString(Game.ColourOne);
               base.WriteString(Game.ColourTwo);
               base.WriteString(Game.ResourcePath);
               base.WriteString(Game.StringThree);
            }
        }
    }
}
