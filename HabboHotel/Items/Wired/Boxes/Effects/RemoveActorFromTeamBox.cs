﻿using Neon.Communication.Packets.Incoming;
using Neon.Communication.Packets.Outgoing.Rooms.Settings;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Rooms.Games.Teams;
using Neon.HabboHotel.Users;
using System.Collections.Concurrent;

namespace Neon.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class RemoveActorFromTeamBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.EffectRemoveActorFromTeam;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public RemoveActorFromTeamBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;

            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            _ = Packet.PopInt();
        }

        public bool Execute(params object[] Params)
        {
            if (Params.Length == 0 || Instance == null)
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

            if (User.Team != TEAM.NONE)
            {
                TeamManager Team = Instance.GetTeamManagerForFreeze();
                if (Team != null)
                {
                    Team.OnUserLeave(User);

                    User.Team = TEAM.NONE;

                    if (User.GetClient().GetHabbo().Effects().CurrentEffect != 0)
                    {
                        User.GetClient().GetHabbo().Effects().ApplyEffect(0);
                    }

                    User.GetClient().SendMessage(new HideUserOnPlaying(false));
                }
            }
            return true;
        }
    }
}