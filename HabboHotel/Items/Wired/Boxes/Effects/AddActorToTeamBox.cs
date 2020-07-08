using Neon.Communication.Packets.Incoming;
using Neon.Communication.Packets.Outgoing.Rooms.Settings;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Rooms.Games.Teams;
using Neon.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class AddActorToTeamBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.EffectAddActorToTeam;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public AddActorToTeamBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;

            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            int Team = Packet.PopInt();

            StringData = Team.ToString();
        }

        public bool Execute(params object[] Params)
        {
            if (Params.Length == 0 || Instance == null || string.IsNullOrEmpty(StringData))
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

            TEAM ToJoin = (int.Parse(StringData) == 1 ? TEAM.RED : int.Parse(StringData) == 2 ? TEAM.GREEN : int.Parse(StringData) == 3 ? TEAM.BLUE : int.Parse(StringData) == 4 ? TEAM.YELLOW : TEAM.NONE);

            TeamManager Team = Instance.GetTeamManagerForFreeze();
            if (Team != null)
            {
                if (Team.CanEnterOnTeam(ToJoin))
                {
                    if (User.Team != TEAM.NONE)
                    {
                        Team.OnUserLeave(User);
                    }

                    User.Team = ToJoin;
                    Team.AddUser(User);

                    if (User.GetClient().GetHabbo().Effects().CurrentEffect != Convert.ToInt32(ToJoin + 39))
                    {
                        User.GetClient().GetHabbo().Effects().ApplyEffect(Convert.ToInt32(ToJoin + 39));
                    }

                    User.GetClient().SendMessage(new HideUserOnPlaying(true));
                }
            }
            return true;
        }
    }
}