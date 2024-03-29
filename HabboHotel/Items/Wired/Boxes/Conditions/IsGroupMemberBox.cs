﻿using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using System.Collections.Concurrent;

namespace Neon.HabboHotel.Items.Wired.Boxes.Conditions
{
    internal class IsGroupMemberBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.ConditionIsGroupMember;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public IsGroupMemberBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Unknown2 = Packet.PopString();
        }

        public bool Execute(params object[] Params)
        {
            if (Params.Length == 0)
            {
                return false;
            }

            Habbo Player = (Habbo)Params[0];
            if (Player == null)
            {
                return false;
            }

            if (Instance.RoomData.Group == null)
            {
                return false;
            }

            if (!Instance.RoomData.Group.IsMember(Player.Id))
            {
                return false;
            }

            return true;
        }
    }
}