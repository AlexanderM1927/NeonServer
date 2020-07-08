using Neon.Communication.Packets.Outgoing.Inventory.Purse;
using Neon.Communication.Packets.Outgoing.LandingView;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;
using System;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class PremiarBonusRare : IChatCommand
    {
        public string PermissionRequired => "command_alert_user";

        public string Parameters => "%username%";

        public string Description => "Recompense a un usuario con bonificaciones.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("¡Ingrese el usuario que desea recompensar!", 34);
                return;
            }

            GameClient Target = NeonEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("¡Vaya, no pudimos encontrar a ese usuario!", 34);
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Target.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Usuario no encontrado! Quizás no esté en línea o en esta sala.", 34);
                return;
            }

            if (Target.GetHabbo().Username == Session.GetHabbo().Username)
            {
                string product = NeonEnvironment.GetDBConfig().DBData["bonus_rare_productdata_name"];
                int baseid = int.Parse(NeonEnvironment.GetDBConfig().DBData["bonus_rare_item_baseid"]);
                int score = Convert.ToInt32(NeonEnvironment.GetDBConfig().DBData["bonus_rare_total_score"]);

                Session.GetHabbo().BonusPoints += 1;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().BonusPoints, score, 101));
                Session.SendMessage(new RoomNotificationComposer("Felicidades ¡Has recibido un punto extra! Tienes ahora: (" + Session.GetHabbo().BonusPoints + ") bono(s)"));
                Session.SendMessage(new BonusRareMessageComposer(Session));
                return;
            }
            if (Target.GetHabbo().Username != Session.GetHabbo().Username)
            {
                string product = NeonEnvironment.GetDBConfig().DBData["bonus_rare_productdata_name"];
                int baseid = int.Parse(NeonEnvironment.GetDBConfig().DBData["bonus_rare_item_baseid"]);
                int score = Convert.ToInt32(NeonEnvironment.GetDBConfig().DBData["bonus_rare_total_score"]);

                Target.GetHabbo().BonusPoints += 1;
                Target.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().BonusPoints, score, 101));
                Target.SendMessage(new RoomNotificationComposer("Felicidades ¡Has recibido un punto extra! Tienes ahora: (" + Target.GetHabbo().BonusPoints + ") bono(s)"));
                Target.SendMessage(new BonusRareMessageComposer(Target));
                Session.SendMessage(new RoomNotificationComposer("Felicidades ¡Has ganado con éxito los puntos de bonificación!"));
            }
        }
    }
}