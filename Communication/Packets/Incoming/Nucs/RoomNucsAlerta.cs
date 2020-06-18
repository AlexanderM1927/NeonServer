using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Neon.Communication.Packets.Outgoing.Nux;
using Neon.Communication.Packets.Outgoing;

namespace Neon.Communication.Packets.Incoming.Nucs
{
    class RoomNucsAlerta : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            var habbo = Session.GetHabbo();

            if (habbo == null) return;
            //if (!habbo.PassedNuxNavigator && !habbo.PassedNuxCatalog && !habbo.PassedNuxChat && !habbo.PassedNuxDuckets && !habbo.PassedNuxItems)
            //{
            //    Session.SendMessage(new NuxAlertComposer("helpBubble/add/BOTTOM_BAR_NAVIGATOR/Este es el navegador de salas, en el, podrás visitar nuevas salas y hacer nuevas amistades."));
            //    habbo.PassedNuxNavigator = true;
            //}

            //if (habbo.PassedNuxNavigator && !habbo.PassedNuxCatalog && !habbo.PassedNuxChat && !habbo.PassedNuxDuckets && !habbo.PassedNuxItems)
            //{
            //    Session.SendMessage(new NuxAlertComposer("helpBubble/add/BOTTOM_BAR_CATALOG/Aquí tienes la tienda, en ella, encontrarás lo necesario para hacer las salas mas chic del hotel!"));
            //    habbo.PassedNuxCatalog = true;
            //}
            //else if (habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && !habbo.PassedNuxChat && !habbo.PassedNuxDuckets && !habbo.PassedNuxItems)
            //{
            //    Session.SendMessage(new NuxAlertComposer("helpBubble/add/CHAT_INPUT/Este es el chat, en el puedes interactuar con el resto de miembros de la comunidad y seleccionar estilos diferentes."));
            //    habbo.PassedNuxChat = true;
            //}
            //else if (habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxChat && !habbo.PassedNuxDuckets && !habbo.PassedNuxItems)
            //{
            //    Session.SendMessage(new NuxAlertComposer("helpBubble/add/DUCKETS_BUTTON/En este apartado, dispondrás de toda la información de tu economía dentro del Hotel"));
            //    habbo.PassedNuxDuckets = true;
            //}
            //else if (habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxChat && habbo.PassedNuxDuckets && !habbo.PassedNuxItems)
            //{
            //    Session.SendMessage(new NuxAlertComposer("helpBubble/add/BOTTOM_BAR_INVENTORY/Aquí está el inventario, en el, todos los furnis que compres, serán almacenados y posteriormente, podrás colocarlos."));
            //    habbo.PassedNuxItems = true;
            //}

            //if (habbo.PassedNuxNavigator && habbo.PassedNuxCatalog && habbo.PassedNuxChat && habbo.PassedNuxDuckets && habbo.PassedNuxItems)
            //{
                Session.SendMessage(new NuxAlertComposer("nux/lobbyoffer/show"));
                habbo._NUX = false;
                using (var dbClient = NeonEnvironment.GetDatabaseManager().GetQueryReactor())
                    dbClient.runFastQuery("UPDATE users SET nux_user = 'false' WHERE id = " + Session.GetHabbo().Id + ";");
                var nuxStatus = new ServerPacket(ServerPacketHeader.NuxUserStatus);
                nuxStatus.WriteInteger(0);
                Session.SendMessage(nuxStatus);
            //}
        }
    }
}
