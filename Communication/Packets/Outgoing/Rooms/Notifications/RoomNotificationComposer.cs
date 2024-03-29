﻿namespace Neon.Communication.Packets.Outgoing.Rooms.Notifications
{
    internal class RoomNotificationComposer : ServerPacket
    {
        public RoomNotificationComposer(string Type)
            : base(ServerPacketHeader.RoomNotificationMessageComposer)
        {
            base.WriteString(Type);
            base.WriteInteger(0);//Count
        }

        public RoomNotificationComposer(string Title, string Message, string Image, string LinkTitle = "", string LinkUrl = "")
            : base(ServerPacketHeader.RoomNotificationMessageComposer)
        {
            base.WriteString(Image);
            base.WriteInteger(string.IsNullOrEmpty(LinkTitle) ? 2 : 4);
            base.WriteString("title");
            base.WriteString(Title);
            base.WriteString("message");
            base.WriteString(Message);

            if (!string.IsNullOrEmpty(LinkTitle))
            {
                base.WriteString("linkUrl");
                base.WriteString(LinkUrl);
                base.WriteString("linkTitle");
                base.WriteString(LinkTitle);
            }
        }

        public RoomNotificationComposer(string title, string content, string url, string urlName, string unknown, int unknown2)
             : base(ServerPacketHeader.RoomNotificationMessageComposer)
        {
            base.WriteString(unknown);
            base.WriteInteger(unknown2);
            // TODO Refactor
            if (unknown2 == 0)
            {
                return;
            }

            switch (unknown2)
            {
                case 0:
                    // nothing more to do
                    break;
                case 1:
                    base.WriteString("errors");
                    base.WriteString(content);
                    break;
                case 4:
                    base.WriteString("title");
                    base.WriteString(title);
                    break;
            }

            if (unknown2 == 3 || unknown2 == 4)
            {
                base.WriteString("message");
                base.WriteString(content);
                base.WriteString("linkUrl");
                base.WriteString(url);
                base.WriteString("linkTitle");
                base.WriteString(urlName);
            }
        }

        public static ServerPacket SendBubble(string image, string message, string linkUrl = "")
        {
            ServerPacket bubbleNotification = new ServerPacket(ServerPacketHeader.RoomNotificationMessageComposer);
            bubbleNotification.WriteString(image);
            bubbleNotification.WriteInteger(string.IsNullOrEmpty(linkUrl) ? 2 : 3);
            bubbleNotification.WriteString("display");
            bubbleNotification.WriteString("BUBBLE");
            bubbleNotification.WriteString("message");
            bubbleNotification.WriteString(message);
            if (string.IsNullOrEmpty(linkUrl))
            {
                return bubbleNotification;
            }

            bubbleNotification.WriteString("linkUrl");
            bubbleNotification.WriteString(linkUrl);
            return bubbleNotification;
        }

        public RoomNotificationComposer(string image, int messageType, string message, string link)
            : base(ServerPacketHeader.RoomNotificationMessageComposer)
        {
            WriteString(image);
            WriteInteger(messageType);
            WriteString("display");
            WriteString("BUBBLE");
            WriteString("message");
            WriteString(message);
            WriteString("linkUrl");
            WriteString(link);

        }

        public static ServerPacket SendCustom(string Message)
        {
            ServerPacket cuz = new ServerPacket(ServerPacketHeader.RoomNotificationMessageComposer);

            cuz.WriteInteger(1);
            cuz.WriteString(Message);

            return cuz;
        }
    }
}
