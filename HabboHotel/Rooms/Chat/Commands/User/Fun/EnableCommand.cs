using Neon.HabboHotel.Rooms.Games.Teams;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    internal class EnableCommand : IChatCommand
    {
        public string PermissionRequired => "command_enable";

        public string Parameters => "%EffectId%";

        public string Description => "Habilitar un efecto en tu personaje.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Usted debe escribir un ID Efecto");
                return;
            }

            RoomUser ThisUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
            if (ThisUser == null)
            {
                return;
            }

            if (ThisUser.RidingHorse)
            {
                Session.SendWhisper("No se puede activar un efecto mientras montas un caballo");
                return;
            }
            else if (ThisUser.Team != TEAM.NONE)
            {
                return;
            }
            else if (ThisUser.isLying)
            {
                return;
            }

            if (!int.TryParse(Params[1], out int EffectId))
            {
                return;
            }

            if (EffectId > int.MaxValue || EffectId < int.MinValue)
            {
                return;
            }

            if (Session.GetHabbo().Rank > 8)
            {
                Session.GetHabbo().LastEffect = EffectId;
                Session.GetHabbo().Effects().ApplyEffect(EffectId);
                return;
            }

            // Staff Effects
            if (EffectId == 102 && Session.GetHabbo().Rank < 5 || EffectId == 187 && Session.GetHabbo().Rank < 5 || EffectId == 593 && Session.GetHabbo().Rank < 5 || EffectId == 596 && Session.GetHabbo().Rank < 5 || EffectId == 598 && Session.GetHabbo().Rank < 5)
            { Session.SendWhisper("Lo sentimos, lamentablemente sólo los staff pueden activar este efecto."); return; }

            // Guide Effects
            if (EffectId == 592 && Session.GetHabbo()._guidelevel < 3 || EffectId == 595 && Session.GetHabbo()._guidelevel < 2 || EffectId == 597 && Session.GetHabbo()._guidelevel < 1)
            { Session.SendWhisper("Lo sentimos, no perteneces al equipo guía, es por ello que no puedes usar este efecto."); return; }

            // Croupier Effect
            if (EffectId == 594 && Session.GetHabbo()._croupier < 1)
            { Session.SendWhisper("Lo sentimos, este enable es sólo para el equipo Croupier de " + NeonEnvironment.GetDBConfig().DBData["hotel.name"] + "."); return; }

            // BAW Effect
            if (EffectId == 599 && Session.GetHabbo()._builder < 1)
            { Session.SendWhisper("Lo sentimos, este enable es sólo para el equipo BAW de " + NeonEnvironment.GetDBConfig().DBData["hotel.name"] + "."); return; }

            // Publicista Effect
            if (EffectId == 600 && Session.GetHabbo()._publicistalevel < 1)
            { Session.SendWhisper("Lo sentimos, este enable es sólo para los publicistas."); return; }

            // VIP Effect
            if (EffectId == 601 && Session.GetHabbo().Rank < 2)
            { Session.SendWhisper("Lo sentimos, este enable es sólo para los VIP."); return; }

            // Ambassador & Rookies Effect
            if (EffectId == 178 && Session.GetHabbo().Rank < 3)
            { Session.SendWhisper("Lo sentimos, este enable es sólo para los embajadores y rookies."); return; }

            Session.GetHabbo().LastEffect = EffectId;
            Session.GetHabbo().Effects().ApplyEffect(EffectId);
        }
    }
}
