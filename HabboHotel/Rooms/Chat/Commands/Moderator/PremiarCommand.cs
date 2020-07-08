using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class PremiarCommand : IChatCommand
    {
        public string PermissionRequired => "command_premiar";

        public string Parameters => "%username% %type% %amount%";

        public string Description => "";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 0)
            {
                Session.SendWhisper("Por favor introduce un nombre de usuario para premiar.", 34);
                return;
            }

            GameClient Target = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Oops, No se ha conseguido este usuario!", 34);
                return;
            }

            Target.SendMessage(NeonEnvironment.GetGame().GetNuxUserGiftsManager().NuxUserGifts.Serialize());
            Session.SendWhisper("Has activado correctamente el premio especial para " + Target.GetHabbo().Username, 34);
            NeonEnvironment.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("premiar", "" + Target.GetHabbo().Username + " ha ganado el evento.", ""));
            Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().GOTWPoints, 103));
            Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Diamonds, 5));
        }
    }
}
