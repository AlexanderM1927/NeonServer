using Neon.Communication.Packets.Outgoing;
using Neon.Communication.Packets.Outgoing.Moderation;
using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.Communication.Packets.Outgoing.Rooms.Polls;
using Neon.HabboHotel.Rooms.Polls;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class MassPollCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_masspoll"; }
        }

        public string Parameters
        {
            get { return "%id%"; }
        }

        public string Description
        {
            get { return "Envia una encuesta a todo el hotel"; }
        }

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduzca la ID de la poll que desee enviar.");
                return;
            }

            RoomPoll poll;
            if (NeonEnvironment.GetGame().GetPollManager().TryGetPollForHotel(int.Parse(Params[1]), out poll))
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
