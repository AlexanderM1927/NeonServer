using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Rooms;

namespace Neon.HabboHotel.Items.Wired
{
    public interface IWiredItem
    {
        Room Instance { get; set; }
        Item Item { get; set; }
        WiredBoxType Type { get; }
        ConcurrentDictionary<int, Item> SetItems { get; set; }
        string StringData { get; set; }
        bool BoolData { get; set; }
        void HandleSave(ClientPacket Packet);
        bool Execute(params object[] Params);
        string ItemsData { get; set; }
    }
}
