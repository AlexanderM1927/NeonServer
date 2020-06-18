using System;
using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Communication.Packets.Outgoing.Nux;
using Neon.Communication.Packets.Outgoing.Rooms.Furni.RentableSpaces;
using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.Communication.Packets.Outgoing.Catalog;
using Neon.Communication.Packets.Outgoing.Users;
using Neon.Database.Interfaces;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class AddTagsToUserCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_addtags"; }
        }

        public string Parameters
        {
            get { return "<usuario> <tag>"; }
        }

        public string Description
        {
            get { return "Añadir TAGs de un usuario."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length != 3)
            {
                Session.SendWhisper("Introduce el nombre del usuario a quien deseas enviar una placa!", 34);
                return;
            }

            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                using (IQueryAdapter dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `users` SET `tag`= '" + Params[2] + "' WHERE `id` = '"+ TargetClient.GetHabbo().Id +"'");

                    TargetClient.GetHabbo().Tags.Add(Params[2]);
                }

                Session.SendMessage(RoomNotificationComposer.SendBubble("definitions", "Has añadido el tag \"" + Params[2] + "\" a " + TargetClient.GetHabbo().Username + " correctamente.", ""));
                TargetClient.SendMessage(RoomNotificationComposer.SendBubble("definitions", Session.GetHabbo().Username + " te ha añadido el tag " + Params[2] +".", ""));

                foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
                {

                    RoomUser.GetClient().SendMessage(new UserTagsComposer(TargetClient.GetHabbo().Id, TargetClient));
                }
            }
        }
    }
}
