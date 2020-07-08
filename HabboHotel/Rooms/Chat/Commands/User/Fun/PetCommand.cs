using Neon.Communication.Packets.Outgoing.Notifications;
using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using System.Text;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class PetCommand : IChatCommand
    {
        public string PermissionRequired => "command_pet";

        public string Parameters => "";

        public string Description => "Transformarte en una mascota. Ver la lista con :pet list.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            RoomUser RoomUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (RoomUser == null)
            {
                return;
            }

            if (!Room.PetMorphsAllowed)
            {
                Session.SendWhisper("El dueño de la sala no permite que te conviertas en una mascota.");
                if (Session.GetHabbo().PetId > 0)
                {
                    Session.SendWhisper("Oops, you still have a morph, un-morphing you.");
                    //Change the users Pet Id.
                    Session.GetHabbo().PetId = 0;

                    //Quickly remove the old user instance.
                    Room.SendMessage(new UserRemoveComposer(RoomUser.VirtualId));

                    //Add the new one, they won't even notice a thing!!11 8-)
                    Room.SendMessage(new UsersComposer(RoomUser));
                }
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Oops, se olvido escribir el nombre de la mascota que desea, use correctamente el ':pet list' y encontrara los disponibles");
                return;
            }

            if (Params[1].ToString().ToLower() == "list")
            {
                Session.SendWhisper("Habbo, Dog, Cat, Terrier, Croc, Bear, Pig, Lion, Rhino, Spider, Turtle, Chick, Frog, Monkey, Horse, Bunny, Pigeon, Demon and Gnome.");
                StringBuilder List = new StringBuilder("");
                List.AppendLine("                              - LISTA DE MASCOTAS -");
                List.AppendLine("Colocando estos parámetros usando el comando :pet podrás transformarte en:");
                List.AppendLine("  l[»]l perro - Transfórmate en un perro.");
                List.AppendLine("  l[»]l gato - Transfórmate en un gato.");
                List.AppendLine("  l[»]l terrier - Transfórmate en un fox terrier.");
                List.AppendLine("  l[»]l cocodrilo - Transfórmate en un cocodrilo.");
                List.AppendLine("  l[»]l oso - Transfórmate en un oso.");
                List.AppendLine("  l[»]l cerdo - Transfórmate en un cerdo.");
                List.AppendLine("  l[»]l león - Transfórmate en un león.");
                List.AppendLine("  l[»]l rinoceronte - Transfórmate en un rinoceronte.");
                List.AppendLine("  l[»]l araña - Transfórmate en una araña.");
                List.AppendLine("  l[»]l tortuga - Transfórmate en una tortuga.");
                List.AppendLine("  l[»]l pollo - Transfórmate en un pollito.");
                List.AppendLine("  l[»]l rana - Transfórmate en una rana.");
                List.AppendLine("  l[»]l mono - Transfórmate en un mono.");
                List.AppendLine("  l[»]l caballo - Transfórmate en un caballo.");
                List.AppendLine("  l[»]l conejo - Transfórmate en un conejo.");
                List.AppendLine("  l[»]l paloma - Transfórmate en una paloma.");
                List.AppendLine("  l[»]l demonio - Transfórmate en un demonio.");
                List.AppendLine("  l[»]l gnomo - Transfórmate en un gnomo.");
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return;
            }

            int TargetPetId = GetPetIdByString(Params[1].ToString());
            if (TargetPetId == 0)
            {
                Session.SendWhisper("Oops, no se ha conseguido ese nombre de la mascota!");
                return;
            }

            //Change the users Pet Id.
            Session.GetHabbo().PetId = (TargetPetId == -1 ? 0 : TargetPetId);

            //Quickly remove the old user instance.
            Room.SendMessage(new UserRemoveComposer(RoomUser.VirtualId));

            //Add the new one, they won't even notice a thing!!11 8-)
            Room.SendMessage(new UsersComposer(RoomUser));

            //Tell them a quick message.
            if (Session.GetHabbo().PetId > 0)
            {
                Session.SendWhisper("Usa ':pet habbo' para volver a la normalidad!");
            }
        }

        private int GetPetIdByString(string Pet)
        {
            switch (Pet.ToLower())
            {
                default:
                    return 0;
                case "habbo":
                    return -1;
                case "perro":
                    return 60;//This should be 0.
                case "gato":
                    return 1;
                case "terrier":
                    return 2;
                case "cocodrilo":
                    return 3;
                case "oso":
                    return 4;
                case "cerdo":
                case "rutlinucs":
                    return 5;
                case "león":
                case "leon":
                    return 6;
                case "rino":
                case "rinoceronte":
                    return 7;
                case "araña":
                case "arana":
                    return 8;
                case "tortuga":
                    return 9;
                case "pollo":
                    return 10;
                case "rana":
                    return 11;
                /*
                  case "drag":
                  case "dragonn":
                      return 12;
                  */
                case "mono":
                    return 14;
                case "caballo":
                    return 15;
                case "conejo":
                    return 17;
                case "paloma":
                case "peruano":
                    return 21;
                case "demonio":
                case "belsebu":
                    return 23;
                case "gnomo":
                    return 26;
            }
        }
    }
}