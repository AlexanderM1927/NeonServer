using Neon.Communication.Packets.Outgoing.Rooms.Furni.Moodlight;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.Data.Moodlight;
using Neon.HabboHotel.Rooms;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.Rooms.Furni.Moodlight
{
    internal class GetMoodlightConfigEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }


            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room Room))
            {
                return;
            }

            if (!Room.CheckRights(Session, true))
            {
                return;
            }

            if (Room.MoodlightData == null)
            {
                foreach (Item item in Room.GetRoomItemHandler().GetWall.ToList())
                {
                    if (item.GetBaseItem().InteractionType == InteractionType.MOODLIGHT)
                    {
                        Room.MoodlightData = new MoodlightData(item.Id);
                    }
                }
            }

            if (Room.MoodlightData == null)
            {
                return;
            }

            Session.SendMessage(new MoodlightConfigComposer(Room.MoodlightData));
        }
    }
}