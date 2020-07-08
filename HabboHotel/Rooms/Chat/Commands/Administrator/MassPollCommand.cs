using Neon.Communication.Packets.Outgoing.Rooms.Polls;
using Neon.HabboHotel.Rooms.Polls;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    internal class MassPollCommand : IChatCommand
    {
        public string PermissionRequired => "command_masspoll";

        public string Parameters => "%id%";

        public string Description => "Envia una encuesta a todo el hotel";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduzca la ID de la poll que desee enviar.");
                return;
            }

            if (NeonEnvironment.GetGame().GetPollManager().TryGetPollForHotel(int.Parse(Params[1]), out RoomPoll poll))
            {
                if (poll.Type == RoomPollType.Poll)
                {
                    NeonEnvironment.GetGame().GetClientManager().SendMessage(new PollOfferComposer(poll));
                }
            }
            return;
        }
    }
}
