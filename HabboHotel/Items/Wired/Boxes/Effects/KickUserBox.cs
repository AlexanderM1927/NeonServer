using Neon.Communication.Packets.Incoming;
using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using System.Collections;
using System.Collections.Concurrent;

namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class KickUserBox : IWiredItem, IWiredCycle
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.EffectKickUser;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public int TickCount { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public int Delay { get; set; }
        public string ItemsData { get; set; }
        private readonly Queue _toKick;

        public KickUserBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
            TickCount = Delay;
            _toKick = new Queue();

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
            string Message = Packet.PopString();

            StringData = Message;
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

            if (TickCount <= 0)
            {
                TickCount = 3;
            }

            if (!_toKick.Contains(Player))
            {
                RoomUser User = Instance.GetRoomUserManager().GetRoomUserByHabbo(Player.Id);
                if (User == null)
                {
                    return false;
                }

                if (Player.Rank >= 7 || Instance.OwnerId == Player.Id)
                {
                    Player.GetClient().SendMessage(new WhisperComposer(User.VirtualId, "Wired Expulsar: Este jugador no se puede expulsar", 0, 0));
                    return false;
                }

                _toKick.Enqueue(Player);
                Player.GetClient().GetHabbo().Effects().ApplyEffect(4);
                Player.GetClient().SendMessage(new WhisperComposer(User.VirtualId, StringData, 0, 0));
            }
            return true;
        }

        public bool OnCycle()
        {
            if (Instance == null)
            {
                return false;
            }

            if (_toKick.Count == 0)
            {
                TickCount = 3;
                return true;
            }

            lock (_toKick.SyncRoot)
            {
                while (_toKick.Count > 0)
                {
                    Habbo Player = (Habbo)_toKick.Dequeue();
                    if (Player == null || !Player.InRoom || Player.CurrentRoom != Instance)
                    {
                        continue;
                    }

                    Player.GetClient().GetHabbo().Effects().ApplyEffect(0);
                    Instance.GetRoomUserManager().RemoveUserFromRoom(Player.GetClient(), true, false);
                }
            }
            TickCount = 3;
            return true;
        }
    }
}