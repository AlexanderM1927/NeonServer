using System;
using Neon.Communication.Packets.Outgoing.Rooms.Furni;
using Neon.HabboHotel.Items.Crafting;

namespace Neon.Communication.Packets.Incoming.Rooms.Furni
{
    class SetCraftingRecipeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            var result = Packet.PopString();

            CraftingRecipe recipe = null;
            foreach (CraftingRecipe Receta in NeonEnvironment.GetGame().GetCraftingManager().CraftingRecipes.Values)
            {
                if (Receta.Result.Contains(result))
                {
                    recipe = Receta;
                    break;
                }
            }

            var Final = NeonEnvironment.GetGame().GetCraftingManager().GetRecipe(recipe.Id);
            if (Final == null) return;
            Session.SendMessage(new CraftingRecipeComposer(Final));
        }

    }
}