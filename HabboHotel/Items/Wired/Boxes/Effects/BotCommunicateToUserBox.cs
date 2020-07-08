using Neon.Communication.Packets.Incoming;
using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using System.Collections.Concurrent;

namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class BotCommunicateToUserBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.EffectBotCommunicatesToUserBox;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public BotCommunicateToUserBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            _ = Packet.PopInt();
            int ChatMode = Packet.PopInt();
            string ChatConfig = Packet.PopString();

            StringData = ChatConfig;
            if (ChatMode == 1)
            {
                BoolData = true;
            }
            else
            {
                BoolData = false;
            }

        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
            {
                return false;
            }

            if (string.IsNullOrEmpty(StringData))
            {
                return false;
            }

            StringData.Split(' ');
            string BotName = StringData.Split('	')[0];
            string Chat = StringData.Split('	')[1];
            _ = StringData.Split('	')[1];
            _ = StringData.Split('	')[1];

            RoomUser User = Instance.GetRoomUserManager().GetBotByName(BotName);
            if (User == null)
            {
                return false;
            }

            Habbo Player = (Habbo)Params[0];
            if (BoolData)
            {
                Player.GetClient().SendMessage(new WhisperComposer(User.VirtualId, Chat, 0, 31));
            }
            else
            {
                User.Chat(Player.GetClient().GetHabbo().Username + ": " + Chat, false, User.LastBubble);
            }

            return true;
        }
    }
}