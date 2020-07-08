using Neon.HabboHotel.GameClients;
using System.Threading;
using System.Threading.Tasks;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class PollCommand : IChatCommand
    {

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            int time = int.Parse(Params[1]);
            string quest = CommandManager.MergeParams(Params, 2);

            if (Params.Length == 0 || Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce la pregunta", 34);
            }
            else
            {

                if (quest == "end")
                {
                    Room.EndQuestion();
                }
                else if (time != -1 || time != 0)
                {
                    Room.StartQuestion(quest);
                    time *= 1000;
                    Task t = Task.Factory.StartNew(() => TaskStopQuestion(Room, time));
                }
                else
                {
                    Room.StartQuestion(quest);
                }
            }
        }

        public void TaskStopQuestion(Room room, int time)
        {
            Thread.Sleep(time);
            room.EndQuestion();
        }

        public string Description =>
            "Realizar una encuesta rápida.";

        public string Parameters =>
            "%time% %question%";

        public string PermissionRequired =>
            "command_give_badge";
    }
}