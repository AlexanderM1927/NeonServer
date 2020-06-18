using System;
using System.Collections.Concurrent;

using Neon.HabboHotel.Rooms;
using Neon.Communication.Packets.Incoming;

namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    class BotCommunicatesToAllBox : IWiredItem, IWiredCycle
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectBotCommunicatesToAllBox; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public int TickCount { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public int Delay { get { return this._delay; } set { this._delay = value; this.TickCount = value + 1; } }
        public string ItemsData { get; set; }

        private long _next;
        private int _delay = 0;
        private bool Requested = false;

        public BotCommunicatesToAllBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
            this.TickCount = Delay;
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            int ChatMode = Packet.PopInt();
            string ChatConfig = Packet.PopString();

            this.StringData = ChatConfig;
            if (ChatMode == 1)
            {
                this.BoolData = true;
            }
            else
            {
                this.BoolData = false;
            }
            int Delay = Packet.PopInt();

            this.Delay = Delay;
            this.TickCount = Delay;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            if (String.IsNullOrEmpty(this.StringData))
                return false;

            this.StringData.Split(' ');

            string BotName = this.StringData.Split('	')[0];
            string Chat = this.StringData.Split('	')[1];

            RoomUser User = this.Instance.GetRoomUserManager().GetBotByName(BotName);
            if (User == null)
                return false;

            if (this.BoolData == true)
            {
                User.Chat(Chat, true, 31);
            }
            else
            {
                User.Chat(Chat, false, 31);
            }


            return true;
        }

        public bool OnCycle()
        {
            if (this._next == 0 || this._next < NeonEnvironment.Now())
                this._next = NeonEnvironment.Now() + this.Delay;


            this.Requested = true;
            this.TickCount = Delay;
            return true;
        }
    }
}