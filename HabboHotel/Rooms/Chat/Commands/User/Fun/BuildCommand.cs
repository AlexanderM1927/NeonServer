using Neon.Communication.Packets.Outgoing.Rooms.Furni;
using Neon.HabboHotel.GameClients;
using Neon.HabboHotel.Items;
using System.Linq;

namespace Neon.HabboHotel.Rooms.Chat.Commands.User.Fan
{
    internal class BuildCommand : IChatCommand
    {
        public string PermissionRequired => "command_build";

        public string Parameters => "%height%";

        public string Description => "";
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string height = Params[1];
            if (Session.GetHabbo().Id == Room.OwnerId)
            {
                if (!Room.CheckRights(Session, true))
                {
                    return;
                }

                Item[] items = Room.GetRoomItemHandler().GetFloor.ToArray();
                foreach (Item Item in items.ToList())
                {
                    _ = NeonEnvironment.GetGame().GetClientManager().GetClientByUserID(Item.UserID);
                    if (Item.GetBaseItem().InteractionType == InteractionType.STACKTOOL)
                    {
                        Room.SendMessage(new UpdateMagicTileComposer(Item.Id, int.Parse(height)));
                    }
                }
            }
        }
    }
}