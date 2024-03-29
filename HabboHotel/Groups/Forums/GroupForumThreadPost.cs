﻿using Neon.Communication.Packets.Outgoing;
using Neon.HabboHotel.Users;

namespace Neon.HabboHotel.Groups.Forums
{
    public class GroupForumThreadPost
    {
        public int Id;
        public int UserId;
        public int Timestamp;
        public string Message;

        public int DeleterId;
        public int DeletedLevel;

        public GroupForumThread ParentThread;
        public GroupForumThreadPost(GroupForumThread parent, int id, int userid, int timestamp, string message, int deletedlevel, int deleterid)
        {

            ParentThread = parent;
            Id = id;
            UserId = userid;
            Timestamp = timestamp;
            Message = message;

            DeleterId = deleterid;
            DeletedLevel = deletedlevel;

        }

        public Habbo GetDeleter()
        {
            return NeonEnvironment.GetHabboById(DeleterId);
        }

        public Habbo GetAuthor()
        {
            return NeonEnvironment.GetHabboById(UserId);
        }

        public void SerializeData(ServerPacket Packet)
        {

            Habbo User = GetAuthor();
            Habbo oculterData = GetDeleter();
            Packet.WriteInteger(Id); //Post Id
            Packet.WriteInteger(ParentThread.Posts.IndexOf(this)); //Post Index

            Packet.WriteInteger(User.Id); //User id
            Packet.WriteString(User.Username); //Username
            Packet.WriteString(User.Look); //User look

            Packet.WriteInteger((int)(NeonEnvironment.GetUnixTimestamp() - Timestamp)); //User message timestamp
            Packet.WriteString(Message); // Message text
            Packet.WriteByte(DeletedLevel * 10); // User message oculted by - level
            Packet.WriteInteger(oculterData != null ? oculterData.Id : 0); // User that oculted message ID
            Packet.WriteString(oculterData != null ? oculterData.Username : "Unknown"); //Oculted message user name
            Packet.WriteInteger(242342340);
            Packet.WriteInteger(ParentThread.GetUserPosts(User.Id).Count); //User messages count
        }

        internal void Save()
        {
            using (Database.Interfaces.IQueryAdapter adap = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                adap.SetQuery("UPDATE group_forums_threads SET deleted_level = @dl, deleter_user_id = @duid WHERE id = @id");
                adap.AddParameter("dl", DeletedLevel);
                adap.AddParameter("duid", DeleterId);
                adap.AddParameter("id", Id);
                adap.RunQuery();
            }
        }
    }
}
