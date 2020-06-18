using Neon.Communication.Packets.Outgoing.Rooms.Notifications;
using Neon.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neon.Database.Interfaces;

namespace Neon.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class VoucherCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_voucher"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Enviale un mensaje de alerta a todos los staff online."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            #region Parametros
            string type = Params[1];
            int value = int.Parse(Params[2]);
            int uses = int.Parse(Params[3]);
            #endregion

            int Voucher = 10;
            string _CaracteresPermitidos = "abcdefghijklmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@$?";
            Byte[] randomBytes = new Byte[Voucher];
            char[] Caracter = new char[Voucher];
            int CuentaPermitida = _CaracteresPermitidos.Length;

            for (int i = 0; i < Voucher; i++)
            {
                Random randomObj = new Random();
                randomObj.NextBytes(randomBytes);
                Caracter[i] = _CaracteresPermitidos[(int)randomBytes[i] % CuentaPermitida];
            }

            var code = new string(Caracter);

            NeonEnvironment.GetGame().GetCatalog().GetVoucherManager().AddVoucher(code, type, value, uses);

            NeonEnvironment.GetGame().GetClientManager().SendMessage(new RoomCustomizedAlertComposer("AVISO: Un nuevo voucher ha sido añadido, para canjearlo, dirígete al catálogo, en la pestaña 'Inicio' en la parte inferior, en el recuadro, introduce lo siguiente: \n\n" +
                "Código: " + code + "\nLa recompensa son: " + type + "\n Puede usarse hasta en " + uses + " ocasiones\n\n ¡Suerte canjeándolo!"));

        }
    }
}
