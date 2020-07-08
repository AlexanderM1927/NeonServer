using Neon.HabboHotel.Items;
using Neon.HabboHotel.Items.Wired;
using System.Linq;

namespace Neon.Communication.Packets.Outgoing.Rooms.Furni.Wired
{
    internal class WiredConditionConfigComposer : ServerPacket
    {
        public WiredConditionConfigComposer(IWiredItem Box)
            : base(ServerPacketHeader.WiredConditionConfigMessageComposer)
        {
            base.WriteBoolean(false);
            if (Box.Type == WiredBoxType.TotalUsersCoincidence) { base.WriteInteger(25); }
            else
            {
                base.WriteInteger(5);
            }

            base.WriteInteger(Box.SetItems.Count);
            foreach (Item Item in Box.SetItems.Values.ToList())
            {
                base.WriteInteger(Item.Id);
            }

            base.WriteInteger(Box.Item.GetBaseItem().SpriteId);
            base.WriteInteger(Box.Item.Id);
            base.WriteString(Box.StringData);

            if (Box.Type == WiredBoxType.ConditionDateRangeActive)
            {
                if (string.IsNullOrEmpty(Box.StringData))
                {
                    Box.StringData = "0;0";
                }

                WriteInteger(2);//Loop
                WriteInteger(Box.StringData != null ? int.Parse(Box.StringData.Split(';')[0]) : 0);
                WriteInteger(Box.StringData != null ? int.Parse(Box.StringData.Split(';')[1]) : 0);

            }

            if (Box.Type == WiredBoxType.ConditionMatchStateAndPosition || Box.Type == WiredBoxType.ConditionDontMatchStateAndPosition)
            {
                if (string.IsNullOrEmpty(Box.StringData))
                {
                    Box.StringData = "0;0;0";
                }

                WriteInteger(3);//Loop
                WriteInteger(Box.StringData != null ? int.Parse(Box.StringData.Split(';')[0]) : 0);
                WriteInteger(Box.StringData != null ? int.Parse(Box.StringData.Split(';')[1]) : 0);
                WriteInteger(Box.StringData != null ? int.Parse(Box.StringData.Split(';')[2]) : 0);
            }
            else if (Box.Type == WiredBoxType.ConditionUserCountInRoom || Box.Type == WiredBoxType.ConditionUserCountDoesntInRoom)
            {
                if (string.IsNullOrEmpty(Box.StringData))
                {
                    Box.StringData = "0;0";
                }

                WriteInteger(2);//Loop
                WriteInteger(Box.StringData != null ? int.Parse(Box.StringData.Split(';')[0]) : 1);
                WriteInteger(Box.StringData != null ? int.Parse(Box.StringData.Split(';')[1]) : 50);
            }

            if (Box.Type == WiredBoxType.ConditionFurniHasNoFurni || Box.Type == WiredBoxType.ConditionFurniHasFurni)
            {
                WriteInteger(1);
            }

            if (Box.Type != WiredBoxType.ConditionUserCountInRoom && Box.Type != WiredBoxType.ConditionUserCountDoesntInRoom && Box.Type != WiredBoxType.ConditionFurniHasNoFurni && Box.Type != WiredBoxType.ConditionFurniHasFurni && Box.Type != WiredBoxType.ConditionDateRangeActive)
            {
                WriteInteger(0);
            }
            else if (Box.Type == WiredBoxType.ConditionFurniHasNoFurni || Box.Type == WiredBoxType.ConditionFurniHasFurni)
            {
                WriteInteger(Box.BoolData ? 1 : 0);
            }
            WriteInteger(0);
            WriteInteger(WiredBoxTypeUtility.GetWiredId(Box.Type));
        }
    }
}