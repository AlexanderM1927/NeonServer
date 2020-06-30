using System;
using System.Collections.Concurrent;

using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using System.Collections;
using Neon.Communication.Packets.Outgoing;

namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    class SendYouTubeVideoBox : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.EffectSendYouTubeVideo; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public SendYouTubeVideoBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            _ = Packet.PopInt();
            string Link = Packet.PopString();

            StringData = Link;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null)
                return false;

            if (string.IsNullOrEmpty(StringData))
                return false;

            ServerPacket packet = new ServerPacket(2);
            packet.WriteString("Youtube");
            packet.WriteString("<iframe id=\"youtube-player\" frameborder=\"0\" allowfullscreen=\"1\" allow=\"autoplay; encrypted - media\" title=\"YouTube video player\" width=\"480\" height=\"270\" src=\"https://www.youtube.com/embed/" + StringData + "?autoplay=1&amp;fs=0&amp;modestbranding=1&amp;rel=0&amp;enablejsapi=1&amp;origin=http%3A%2F%2Fhabblive.in&amp;widgetid=1\"></iframe>");

            if (Player.GetClient().wsSession != null)
                Player.GetClient().wsSession.send(packet);

            return true;
        }
    }
}
