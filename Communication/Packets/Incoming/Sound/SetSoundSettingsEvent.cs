﻿using Neon.Database.Interfaces;


namespace Neon.Communication.Packets.Incoming.Sound
{
    internal class SetSoundSettingsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Volume = "";
            for (int i = 0; i < 3; i++)
            {
                int Vol = Packet.PopInt();
                if (Vol < 0 || Vol > 100)
                {
                    Vol = 100;
                }

                if (i < 2)
                {
                    Volume += Vol + ",";
                }
                else
                {
                    Volume += Vol;
                }
            }

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE users SET volume = @volume WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                dbClient.AddParameter("volume", Volume);
                dbClient.RunQuery();
            }
        }
    }
}
