using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class SexCommand : IChatCommand
    {
        public string PermissionRequired => "command_sex";
        public string Parameters => "%username%";
        public string Description => "Hacer sexo con otro usuario";
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Ingrese el nombre de usuario de la persona con la que desea tener relaciones sexuales.");
                return;
            }
            GameClient TargetClient = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Esto es como necrofilia >:(");
                return;
            }
            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Se produjo un error al encontrar a ese usuario, tal vez están fuera de línea o no en esta sala.");
            }
            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("El nombre de esto es masturbación. El amor propio es todo!");
                return;
            }
            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
            {
                return;
            }

            if (!((Math.Abs(TargetUser.X - ThisUser.X) >= 2) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 2)))
            {
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Gira " + Params[1] + " y comienza a tener sexo con ellos.*", 0, ThisUser.LastBubble));
                System.Threading.Thread.Sleep(1000);
                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "Se inclina y comienza a tener relaciones sexuales con " + Session.GetHabbo().Username + "*", 0, ThisUser.LastBubble));
                System.Threading.Thread.Sleep(1000);
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "Golpea " + Params[1] + " culo y jalar el cabello*", 0, ThisUser.LastBubble));
                System.Threading.Thread.Sleep(1000);
                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "Hay algo extraño en el aire, y hay algo mojado en el piso.", 0, ThisUser.LastBubble));
                System.Threading.Thread.Sleep(1000);
                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "Se derrumba en el suelo, cansado y agotado..", 0, ThisUser.LastBubble));
                TargetUser.Statusses.Add("lay", "0.1");
                TargetUser.isLying = true;
                TargetUser.UpdateNeeded = true;
            }
            else
            {
                Session.SendWhisper("Nadie tiene un pene de ese tamaño, acércate.");
                return;
            }
        }
    }
}