﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;



using Neon.Communication.Packets.Outgoing.Rooms.Engine;

using Neon.Database.Interfaces;
using Neon.Communication.Packets.Outgoing.Rooms.Session;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using System.Threading.Tasks;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class ResetScoreBoard : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_resetsc"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Lets you reset your wired scoreboard"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("You can only reset a scoreboard in a room you own!");
                return;
            }

            //Unload the Room
            Room R = null;
            if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Room.Id, out R))
                return;
            List<RoomUser> UsersToReturn = Room.GetRoomUserManager().GetRoomUsers().ToList();
            NeonEnvironment.GetGame().GetNavigator().Init();
            NeonEnvironment.GetGame().GetRoomManager().UnloadRoom(R, true);
            foreach (RoomUser User2 in UsersToReturn)
            {
                if (User2 == null || User2.GetClient() == null)
                    continue;

                Task.Run(async delegate
                {
                    await Task.Delay(1000);
                    User.GetClient().SendMessage(new RoomForwardComposer(Room.Id));
                    using (IQueryAdapter Adapter = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        Adapter.SetQuery("DELETE FROM `wired_scorebord` WHERE(`roomid`= @roomid)");
                        Adapter.AddParameter("roomid", Room.Id);
                        Adapter.RunQuery();
                    }
                });

                Session.SendMessage(new RoomNotificationComposer("Scoreboard Alert", "\rYou have just reset your scoreboards!\r\n<i>Scoreboard Reset!</i>", $"figure/{Session.GetHabbo().Look}&head_direction=3&action=wav&gesture=sml&direction=2", "OK!", "event:close"));

            }

        }
    }
}
