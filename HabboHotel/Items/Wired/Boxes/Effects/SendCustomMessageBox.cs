﻿using Neon.Communication.Packets.Incoming;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using System.Collections.Concurrent;

namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class SendCustomMessageBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.SendCustomMessageBox;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public SendCustomMessageBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Message = Packet.PopString();

            StringData = Message;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
            {
                return false;
            }

            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null || string.IsNullOrWhiteSpace(StringData))
            {
                return false;
            }

            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
            {
                return false;
            }

            string[] Message = StringData.Split('_');

            if (Message[0] == "1")
            {
                Player.GetClient().SendMessage(new RoomNotificationComposer(Message[1],
                Message[2], "gifts_event", Message[3], "event:" + Message[4]));
            }

            if (Message[0] == "2")
            {
                Player.GetClient().SendMessage(new RoomCustomizedAlertComposer(Message[1]));
            }

            if (Message[0] == "3")
            {
                Player.GetClient().SendMessage(new MassEventComposer(Message[1]));
            }

            if (Message[0] == "4")
            {
                Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble(Message[1], Message[2], Message[3]));
            }

            if (Message[0] == "5")
            {
                Player.GetClient().SendMessage(new WiredSmartAlertComposer(Message[1]));
            }

            //if (Message[0] == "6")
            //{
            //    string eventillo = "<img src=\"icon_256.png\" alt=\"xD\" height=\"15\" width=\"15\"></img>";
            //    Player.GetClient().SendMessage(new UserNameChangeComposer(Instance.Id, User.VirtualId, eventillo));
            //    Player.GetClient().SendChat("Hay un nuevo evento, haz <a href='event:navigator/goto/3'><b>click aquí</b></a> para ir al evento.        ", 33);
            //}
            return true;
        }
    }
}