﻿//using Neon.Communication.Packets.Outgoing;

//namespace Neon.Communication.Packets.Incoming.Catalog
//{
//    class FurniMaticRewardsEvent : IPacketEvent
//    {
//        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
//        {
//            var response = new ServerPacket(ServerPacketHeader.FurniMaticRewardsComposer);
//            response.WriteInteger(3);
//            for (int i = 3; i >= 1; i--)
//            {
//                response.WriteInteger(i);
//                if (i <= 1) response.WriteInteger(1);
//                else if (i == 2) response.WriteInteger(15);
//                else if (i == 3) response.WriteInteger(35);
//                var rewards = NeonEnvironment.GetGame().GetFurniMaticRewardsMnager().GetRewardsByLevel(i);
//                response.WriteInteger(rewards.Count);
//                foreach (var reward in rewards)
//                {
//                    response.WriteString(reward.GetBaseItem().ItemName);
//                    response.WriteInteger(reward.DisplayId);
//                    response.WriteString(reward.GetBaseItem().Type.ToString().ToLower());
//                    response.WriteInteger(reward.GetBaseItem().SpriteId);
//                }
//            }

//            Session.SendMessage(response);
//        }
//    }
//}

using Neon.Communication.Packets.Outgoing;

namespace Neon.Communication.Packets.Incoming.Catalog
{
    internal class FurniMaticRewardsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ServerPacket response = new ServerPacket(ServerPacketHeader.FurniMaticRewardsComposer);
            response.WriteInteger(5);
            for (int i = 5; i >= 1; i--)
            {
                response.WriteInteger(i);
                if (i <= 1)
                {
                    response.WriteInteger(1);
                }
                else if (i == 2)
                {
                    response.WriteInteger(5);
                }
                else if (i == 3)
                {
                    response.WriteInteger(20);
                }
                else if (i == 4)
                {
                    response.WriteInteger(50);
                }
                else if (i == 5)
                {
                    response.WriteInteger(100);
                }

                System.Collections.Generic.List<HabboHotel.Catalog.FurniMatic.FurniMaticRewards> rewards = NeonEnvironment.GetGame().GetFurniMaticRewardsMnager().GetRewardsByLevel(i);
                response.WriteInteger(rewards.Count);
                foreach (HabboHotel.Catalog.FurniMatic.FurniMaticRewards reward in rewards)
                {
                    response.WriteString(reward.GetBaseItem().ItemName);
                    response.WriteInteger(reward.DisplayId);
                    response.WriteString(reward.GetBaseItem().Type.ToString().ToLower());
                    response.WriteInteger(reward.GetBaseItem().SpriteId);
                }
            }

            Session.SendMessage(response);
        }
    }
}

