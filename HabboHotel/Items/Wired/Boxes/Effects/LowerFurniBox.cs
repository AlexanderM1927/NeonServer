using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Rooms;
using System.Collections.Concurrent;
using System.Linq;


namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class LowerFurniBox : IWiredItem, IWiredCycle
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.EffectLowerFurni;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public int TickCount { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public int Delay { get => _delay; set { _delay = value; TickCount = value; } }
        public string ItemsData { get; set; }

        private long _next;
        private int _delay = 0;
        private bool Requested = false;

        public LowerFurniBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            SetItems.Clear();
            int Unknown = Packet.PopInt();
            string Unknown2 = Packet.PopString();

            int FurniCount = Packet.PopInt();
            for (int i = 0; i < FurniCount; i++)
            {
                Item SelectedItem = Instance.GetRoomItemHandler().GetItem(Packet.PopInt());
                if (SelectedItem != null)
                {
                    SetItems.TryAdd(SelectedItem.Id, SelectedItem);
                }
            }

            int Delay = Packet.PopInt();
            this.Delay = Delay;
        }

        public bool Execute(params object[] Params)
        {
            if (SetItems.Count == 0)
            {
                return false;
            }

            if (_next == 0 || _next < NeonEnvironment.Now())
            {
                _next = NeonEnvironment.Now() + Delay;
            }

            if (!Requested)
            {
                TickCount = Delay;
                Requested = true;
            }
            return true;
        }

        public bool OnCycle()
        {
            if (Instance == null || !Requested || _next == 0)
            {
                return false;
            }

            long Now = NeonEnvironment.Now();
            if (_next < Now)
            {
                foreach (Item Item in SetItems.Values.ToList())
                {
                    if (Item == null)
                    {
                        continue;
                    }

                    if (!Instance.GetRoomItemHandler().GetFloor.Contains(Item))
                    {
                        continue;
                    }

                    Item.GetZ--;
                    Item.UpdateState();
                }

                _next = 0;
                return true;
            }
            return false;
        }
    }
}