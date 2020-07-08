using Neon.Communication.Packets.Incoming;
using Neon.Communication.Packets.Outgoing.Messenger;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Users;
using System.Collections.Concurrent;

namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class ProgressUserAchievementBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.EffectProgressUserAchievement;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public ProgressUserAchievementBox(Room Instance, Item Item)
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


            Habbo Owner = NeonEnvironment.GetHabboById(Item.UserID);
            if (Owner == null || Owner.Rank < 6)
            {
                StringData = "";
                Owner.GetClient().SendWhisper("No sé quién te ha dado esto pero no deberías estar jugando con juguetes de mayores.", 34);
                NeonEnvironment.GetGame().GetClientManager().StaffAlert1(new RoomInviteComposer(int.MinValue, Owner.Username + " está utilizando sin permiso un Wired de Puntos de Recompensa."));
            }
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

            string[] Message = StringData.Split('-');
            NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(User.GetClient(), "ACH_" + Message[0], int.Parse(Message[1]));
            return true;
        }
    }
}