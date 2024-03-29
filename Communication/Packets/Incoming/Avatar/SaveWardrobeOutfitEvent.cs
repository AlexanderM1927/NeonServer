﻿using Neon.Database.Interfaces;

namespace Neon.Communication.Packets.Incoming.Avatar
{
    internal class SaveWardrobeOutfitEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int SlotId = Packet.PopInt();
            string Look = NeonEnvironment.GetGame().GetAntiMutant().RunLook(Packet.PopString());
            string Gender = Packet.PopString();

            using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT null FROM `user_wardrobe` WHERE `user_id` = " + Session.GetHabbo().Id + " AND `slot_id` = @slot");
                dbClient.AddParameter("slot", SlotId);

                if (dbClient.getRow() != null)
                {
                    dbClient.SetQuery("UPDATE `user_wardrobe` SET `look` = @look, `gender` = @gender WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND `slot_id` = @slot LIMIT 1");
                    dbClient.AddParameter("slot", SlotId);
                    dbClient.AddParameter("look", Look);
                    dbClient.AddParameter("gender", Gender.ToUpper());
                    dbClient.RunQuery();
                }
                else
                {
                    dbClient.SetQuery("INSERT INTO `user_wardrobe` (`user_id`,`slot_id`,`look`,`gender`) VALUES ('" + Session.GetHabbo().Id + "',@slot,@look,@gender)");
                    dbClient.AddParameter("slot", SlotId);
                    dbClient.AddParameter("look", Look);
                    dbClient.AddParameter("gender", Gender.ToUpper());
                    dbClient.RunQuery();
                }
            }
        }
    }
}
