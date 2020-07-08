using Neon.Communication.Packets.Outgoing.Rooms.Chat;
using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.Communication.Packets.Outgoing.Rooms.Furni;
using Neon.Communication.Packets.Outgoing.Rooms.Poll;
using Neon.Communication.Packets.Outgoing.Rooms.Polls;
using Neon.HabboHotel.Items.Wired;
using Neon.HabboHotel.Rooms;
using Neon.HabboHotel.Rooms.Polls;

namespace Neon.Communication.Packets.Incoming.Rooms.Engine
{
    internal class GetRoomEntryDataEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            if (Session.GetHabbo().InRoom)
            {

                if (!NeonEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room OldRoom))
                {
                    return;
                }

                if (OldRoom.GetRoomUserManager() != null)
                {
                    OldRoom.GetRoomUserManager().RemoveUserFromRoom(Session, false, false);
                }
            }

            if (!Session.GetHabbo().BlnInv)
            {
                if (!Room.GetRoomUserManager().AddAvatarToRoom(Session))
                {
                    Room.GetRoomUserManager().RemoveUserFromRoom(Session, false, false);
                    return;//TODO: Remove?
                }
            }
            else
            {
                Session.GetHabbo().BlnInv = !Session.GetHabbo().BlnInv;
            }

            Room.SendObjects(Session);

            try
            {
                if (Session.GetHabbo().GetMessenger() != null)
                {
                    Session.GetHabbo().GetMessenger().OnStatusChanged(true);
                }
            }
            catch { }

            if (Session.GetHabbo().GetStats().QuestID > 0)
            {
                NeonEnvironment.GetGame().GetQuestManager().QuestReminder(Session, Session.GetHabbo().GetStats().QuestID);
            }

            Session.SendMessage(new RoomEntryInfoComposer(Room.RoomId, Room.CheckRights(Session, true)));
            Session.SendMessage(new RoomVisualizationSettingsComposer(Room.WallThickness, Room.FloorThickness, NeonEnvironment.EnumToBool(Room.Hidewall.ToString())));

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);

            if (ThisUser != null && Session.GetHabbo().PetId == 0)
            {
                Room.SendMessage(new UserChangeComposer(ThisUser, false));
            }

            if (!Session.GetHabbo().Effects().HasEffect(0) && Session.GetHabbo().Rank < 2)
            {
                Session.GetHabbo().Effects().ApplyEffect(0);
            }

            Session.SendMessage(new RoomEventComposer(Room.RoomData, Room.RoomData.Promotion));

            if (Session.GetHabbo().Rank > 8 && !Session.GetHabbo().StaffOk)
            {
                Session.SendMessage(new GnomeBoxComposer(0));
            }

            if (Room.poolQuestion != string.Empty)
            {
                Session.SendMessage(new QuickPollMessageComposer(Room.poolQuestion));

                if (Room.yesPoolAnswers.Contains(Session.GetHabbo().Id) || Room.noPoolAnswers.Contains(Session.GetHabbo().Id))
                {
                    Session.SendMessage(new QuickPollResultsMessageComposer(Room.yesPoolAnswers.Count, Room.noPoolAnswers.Count));
                }
            }

            if (Room.GetWired() != null)
            {
                Room.GetWired().TriggerEvent(WiredBoxType.TriggerRoomEnter, Session.GetHabbo());
            }

            if (Room.ForSale && Room.SalePrice > 0 && (Room.GetRoomUserManager().GetRoomUserByHabbo(Room.OwnerName) != null))
            {
                Session.SendWhisper("Esta Sala esta en venta, en " + Room.SalePrice + " Duckets. Escribe :buyroom si deseas comprarla!");
            }
            else if (Room.ForSale && Room.GetRoomUserManager().GetRoomUserByHabbo(Room.OwnerName) == null)
            {
                foreach (RoomUser _User in Room.GetRoomUserManager().GetRoomUsers())
                {
                    if (_User.GetClient() != null && _User.GetClient().GetHabbo() != null && _User.GetClient().GetHabbo().Id != Session.GetHabbo().Id)
                    {
                        _User.GetClient().SendWhisper("Esta Sala ya no se encuentra a la venta.");
                    }
                }
                Room.ForSale = false;
                Room.SalePrice = 0;
            }

            if (NeonEnvironment.GetGame().GetPollManager().TryGetPollForRoom(Room.Id, out RoomPoll poll))
            {
                if (poll.Type == RoomPollType.Poll)
                {
                    Session.SendMessage(new PollOfferComposer(poll));
                }
            }

            if (NeonEnvironment.GetUnixTimestamp() < Session.GetHabbo().FloodTime && Session.GetHabbo().FloodTime != 0)
            {
                Session.SendMessage(new FloodControlComposer((int)Session.GetHabbo().FloodTime - (int)NeonEnvironment.GetUnixTimestamp()));
            }
        }
    }
}
