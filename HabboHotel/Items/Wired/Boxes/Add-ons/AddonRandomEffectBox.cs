using Neon.Communication.Packets.Incoming;
using Neon.HabboHotel.Rooms;
using System.Collections.Concurrent;

namespace Neon.HabboHotel.Items.Wired.Boxes.Add_ons
{
    internal class AddonRandomEffectBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.AddonRandomEffect;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public AddonRandomEffectBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();

            if (SetItems.Count > 0)
            {
                SetItems.Clear();
            }
        }

        public void HandleSave(ClientPacket Packet)
        {

        }

        public bool Execute(params object[] Params)
        {
            return true;
        }
    }
}