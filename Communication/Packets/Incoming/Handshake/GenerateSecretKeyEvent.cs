﻿
using Neon.Communication.Encryption;
using Neon.Communication.Encryption.Crypto.Prng;
using Neon.Communication.Packets.Outgoing.Handshake;

namespace Neon.Communication.Packets.Incoming.Handshake
{
    public class GenerateSecretKeyEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string CipherPublickey = Packet.PopString();

            BigInteger SharedKey = HabboEncryptionV2.CalculateDiffieHellmanSharedKey(CipherPublickey);
            if (SharedKey != 0)
            {
                Session.RC4Client = new ARC4(SharedKey.getBytes());
                Session.SendMessage(new SecretKeyComposer(HabboEncryptionV2.GetRsaDiffieHellmanPublicKey()));
            }
            else
            {
                Session.SendNotification("Se ha producido un error, por favor inicie sesion nuevamente!");
                return;
            }
        }
    }
}