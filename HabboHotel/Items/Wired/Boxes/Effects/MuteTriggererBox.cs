using Neon.Communication.Packets.Incoming;
using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using System.Collections.Concurrent;

namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class MuteTriggererBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.EffectMuteTriggerer;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public MuteTriggererBox(Room Instance, Item Item)
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
            if (SetItems.Count > 0)
            {
                SetItems.Clear();
            }

            int Unknown = Packet.PopInt();
            int Time = Packet.PopInt();
            string Message = Packet.PopString();

            StringData = Time + ";" + Message;
        }

        public bool Execute(params object[] Params)
        {
            if (Params.Length != 1)
            {
                return false;
            }

            Habbo Player = (Habbo)Params[0];
            if (Player == null)
            {
                return false;
            }

            RoomUser User = Instance.GetRoomUserManager().GetRoomUserByHabbo(Player.Id);
            if (User == null)
            {
                return false;
            }

            if (Player.GetPermissions().HasRight("mod_tool") || Instance.OwnerId == Player.Id)
            {
                Player.GetClient().SendMessage(new WhisperComposer(User.VirtualId, "Wired Mute Exception: Unmutable Player", 0, 0));
                return false;
            }

            int Time = (StringData != null ? int.Parse(StringData.Split(';')[0]) : 0);
            string Message = (StringData != null ? (StringData.Split(';')[1]) : "No message!");

            if (Time > 0)
            {
                Player.GetClient().SendMessage(new WhisperComposer(User.VirtualId, "Wired Mute: Muted for " + Time + "! Message: " + Message, 0, 0));
                if (!Instance.MutedUsers.ContainsKey(Player.Id))
                {
                    Instance.MutedUsers.Add(Player.Id, (NeonEnvironment.GetUnixTimestamp() + (Time * 60)));
                }
                else
                {
                    Instance.MutedUsers.Remove(Player.Id);
                    Instance.MutedUsers.Add(Player.Id, (NeonEnvironment.GetUnixTimestamp() + (Time * 60)));
                }
            }

            return true;
        }
    }
}