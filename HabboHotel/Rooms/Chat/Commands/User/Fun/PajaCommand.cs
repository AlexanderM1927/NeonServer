using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using System.Threading;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class PajaCommand : IChatCommand
    {
        public string PermissionRequired => "command_paja";

        public string Parameters => "";
        public string Description => "Te estas pajeando";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendNotification("¿Te gustaria hacerte una buena jalada de ganzo?\n\n" +
                 "Para confirmar, escriba \":paja si\". \n\n Después de haber hecho usted no puede volver!\n\n(Si no quieres jalarte el ganzo, ignora este mensaje! ;) )\n\n");
                return;
            }
            RoomUser roomUserByHabbo = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (roomUserByHabbo == null)
            {
                return;
            }

            if (Params.Length == 2 && Params[1].ToString() == "si")
            {

                Room.SendMessage(new ChatComposer(roomUserByHabbo.VirtualId, "* Se saca el pene *", 0, 3), false);
                Thread.Sleep(2000);
                Room.SendMessage(new ChatComposer(roomUserByHabbo.VirtualId, "*Se pajea tan rapido que queda con cojonera*", 0, 3), false);
                Thread.Sleep(2000);
                roomUserByHabbo.ApplyEffect(602);
                Room.SendMessage(new ChatComposer(roomUserByHabbo.VirtualId, "*Se ensucia de cosa blanca*", 0, 3), false);
                Thread.Sleep(2000);
                roomUserByHabbo.ApplyEffect(0);
            }

        }
    }
}