using Neon.Communication.Packets.Outgoing.Rooms.Engine;
using Neon.Communication.Packets.Outgoing.Rooms.FloorPlan;
using Neon.HabboHotel.Items;
using Neon.HabboHotel.Rooms;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Neon.Communication.Packets.Incoming.Rooms.FloorPlan
{
    internal class FloorPlanEditorRoomPropertiesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
            {
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
            {
                return;
            }

            if (Room.GetGameMap().Model == null)
            {
                return;
            }

            List<Point> Squares = new List<Point>();
            Room.GetRoomItemHandler().GetFloor.ToList().ForEach(Item =>
            {
                Item.GetCoords.ForEach(Point =>
                {
                    if (!Squares.Contains(Point))
                    {
                        Squares.Add(Point);
                    }
                });
            });

            Session.SendMessage(new FloorPlanFloorMapComposer(Squares));
            Session.SendMessage(new FloorPlanSendDoorComposer(Room.GetGameMap().Model.DoorX, Room.GetGameMap().Model.DoorY, Room.GetGameMap().Model.DoorOrientation));
            Session.SendMessage(new RoomVisualizationSettingsComposer(Room.WallThickness, Room.FloorThickness, NeonEnvironment.EnumToBool(Room.Hidewall.ToString())));

            Squares.Clear();
            Squares = null;
        }
    }
}