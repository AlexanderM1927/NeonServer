using Neon.Communication.Packets.Outgoing.Nux;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class CustomLegit : IChatCommand
    {
        public string PermissionRequired => "command_info";

        public string Parameters => "";

        public string Description => "Qué nos deparará el destino...";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Session.SendMessage(new NuxAlertComposer("helpBubble/add/CHAT_INPUT/Death awaits us..."));
            Session.SendMessage(new NuxAlertComposer("nux/lobbyoffer/hide"));
            NeonEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_Login", 1);
        }
    }
}