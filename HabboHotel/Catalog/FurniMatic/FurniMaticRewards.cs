using Neon.HabboHotel.Items;

namespace Neon.HabboHotel.Catalog.FurniMatic
{
    public class FurniMaticRewards
    {
        public int DisplayId;
        public int BaseId;
        public int Level;

        public FurniMaticRewards(int displayId, int baseId, int level)
        {
            DisplayId = displayId;
            BaseId = baseId;
            Level = level;
        }

        public ItemData GetBaseItem()
        {
            if (NeonEnvironment.GetGame().GetItemManager().GetItem(BaseId, out ItemData data))
            {
                return data;
            }

            return null;
        }
    }
}