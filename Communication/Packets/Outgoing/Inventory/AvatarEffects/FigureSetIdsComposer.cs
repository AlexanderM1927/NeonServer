#region

using Neon.HabboHotel.Users.Clothing.Parts;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Neon.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
    internal class FigureSetIdsComposer : ServerPacket
    {
        public FigureSetIdsComposer(ICollection<ClothingParts> ClothingParts)
            : base(ServerPacketHeader.FigureSetIdsMessageComposer)
        {
            WriteInteger(ClothingParts.Count);
            foreach (ClothingParts Part in ClothingParts.ToList())
            {
                WriteInteger(Part.PartId);
            }

            WriteInteger(ClothingParts.Count);
            foreach (ClothingParts Part in ClothingParts.ToList())
            {
                WriteString(Part.Part);
            }
        }
    }
}