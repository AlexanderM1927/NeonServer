/*using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.Communication.Packets.Outgoing.Inventory.Furni;
using System.Globalization;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    class SetzCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_setz";
            }
        }
        public string Parameters
        {
            get
            {
                return "%message%";
            }
        }
        public string Description
        {
            get
            {
                return "Set the Stack Height.";
            }
        }
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (!Room.CheckRights(Session, false, false))
            {
                Session.SendNotification("No tiene permiso para el comando `stack_height`");
                return;
            }

            RoomUser user = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (user == null)
            {
                Session.SendNotification("Usuario no encontrado.");
                return;
            }


            if (Params.Length < 2)
            {
                Session.SendWhisper("Ingrese un valor numérico o escriba ':setz -' para desactivarlo", 34);
                return;
            }

            if (Params[1] == "-")
            {
                Session.SendWhisper("Altura de la pila deshabilitada", 34);
                Session.GetHabbo().ForceHeight = -1;
                return;
            }

            double value;
            bool checkIfParsable = Double.TryParse(Params[1], out value);
            if (checkIfParsable == false)
            {
                Session.SendWhisper("Ingrese un valor numérico o escriba ':setz -' para desactivarlo", 34);
                return;
            }


            double HeightValue = Convert.ToDouble(Params[1]);
            if (HeightValue < 0 || HeightValue > 100)
            {
                Session.SendWhisper("Por favor, introduzca un valor entre 0 y 100", 34);
                return;
            }

            Session.GetHabbo().ForceHeight = HeightValue;
            Session.SendWhisper("La altura es: " + Convert.ToString(HeightValue), 34);
        }
    }
}*/