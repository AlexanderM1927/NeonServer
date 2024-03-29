﻿namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    internal class ForceSitCommand : IChatCommand
    {
        public string PermissionRequired => "command_forcesit";

        public string Parameters => "%username%";

        public string Description => "Obliga a otro usuario a sentarse";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Oops, usted olvido al usuario que desea forzar a sentarse!");
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
            if (User == null)
            {
                return;
            }

            if (User.Statusses.ContainsKey("lie") || User.isLying || User.RidingHorse || User.IsWalking)
            {
                return;
            }

            if (!User.Statusses.ContainsKey("sit"))
            {
                if ((User.RotBody % 2) == 0)
                {
                    if (User == null)
                    {
                        return;
                    }

                    try
                    {
                        User.Statusses.Add("sit", "1.0");
                        User.Z -= 0.35;
                        User.isSitting = true;
                        User.UpdateNeeded = true;
                    }
                    catch { }
                }
                else
                {
                    User.RotBody--;
                    User.Statusses.Add("sit", "1.0");
                    User.Z -= 0.35;
                    User.isSitting = true;
                    User.UpdateNeeded = true;
                }
            }
            else if (User.isSitting == true)
            {
                User.Z += 0.35;
                User.Statusses.Remove("sit");
                User.Statusses.Remove("1.0");
                User.isSitting = false;
                User.UpdateNeeded = true;
            }
        }
    }
}
