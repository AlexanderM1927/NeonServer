using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Neon.HabboHotel.GameClients;
using Neon.Communication.Packets.Outgoing.Rooms.Session;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class SummonAll : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_summonall"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Trae a un usuario a todo cristo."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            foreach (GameClient Client in NeonEnvironment.GetGame().GetClientManager().GetClients.ToList())
            {
                if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().Username == Session.GetHabbo().Username)
                    continue;

                if (Client.GetHabbo().InRoom && Client.GetHabbo().CurrentRoomId != Session.GetHabbo().CurrentRoomId)
                {
                    Client.SendMessage(new RoomForwardComposer(Session.GetHabbo().CurrentRoomId));
                    Client.SendNotification("¡Acabas de ser atraído por " + Session.GetHabbo().Username + "!");
                }
                else if (!Client.GetHabbo().InRoom)
                {
                    Client.SendMessage(new RoomForwardComposer(Session.GetHabbo().CurrentRoomId));
                    Client.SendNotification("¡Acabas de ser atraído por " + Session.GetHabbo().Username + "!");
                }
                else if (Client.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                {
                    Client.SendWhisper("Vaya, parece que se acaba de traer a todo el hotel en la sala en la que te encuentras...", 34);
                }
            }

            Session.SendWhisper("Acabas de atraer a todo el puto hotel men.");


            }
        }
    }
