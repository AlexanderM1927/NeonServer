using Neon.Communication.Packets.Outgoing;
using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.Communication.Packets.Outgoing.Rooms.Engine;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User
{
    internal class EmojiCommand : IChatCommand
    {
        public string PermissionRequired => "";
        public string Parameters => "";
        public string Description => "Numero de 1-199. Manda un emoji";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Oops, debes escribir un numero de 1-199! Para ver la lista de emoji escribe :emoji lista");
                return;
            }
            string emoji = Params[1];

            if (emoji.Equals("lista"))
            {
                ServerPacket notif = new ServerPacket(ServerPacketHeader.NuxAlertMessageComposer);
                notif.WriteString("habbopages/chat/emoji/emoji.txt");
                Session.SendMessage(notif);
            }
            else
            {
                bool isNumeric = int.TryParse(emoji, out int emojiNum);
                if (isNumeric)
                {
                    switch (emojiNum)
                    {
                        default:
                            bool isValid = true;
                            if (emojiNum < 1)
                            {
                                isValid = false;
                            }

                            if (emojiNum > 199)
                            {
                                isValid = false;
                            }

                            if (isValid)
                            {
                                string Username;
                                RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
                                if (emojiNum < 10)
                                {
                                    Username = "<img src='/swf/c_images/emoji/Emoji_Smiley/Emoji Smiley-0" + emojiNum + ".png' height='20' width='20'><br>    >";
                                }
                                else
                                {
                                    Username = "<img src='/swf/c_images/emoji/Emoji_Smiley/Emoji Smiley-" + emojiNum + ".png' height='20' width='20'><br>    >";
                                }
                                if (Room != null)
                                {
                                    Room.SendMessage(new UserNameChangeComposer(Session.GetHabbo().CurrentRoomId, TargetUser.VirtualId, Username));
                                }

                                string Message = " ";
                                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, Message, 0, TargetUser.LastBubble));
                                TargetUser.SendNamePacket();

                            }
                            else
                            {
                                Session.SendWhisper("Emoji invalido, debe ser numero de 1-199. Para ver la lista de emojis escribe ':emoji lista'");
                            }

                            break;
                    }
                }
                else
                {
                    Session.SendWhisper("Emoji invalido, debe ser numero de 1-199. Para ver la lista de emojis escribe ':emoji lista'");
                }
            }
        }
    }
}
