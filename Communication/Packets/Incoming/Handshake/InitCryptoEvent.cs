
using Neon.Communication.Encryption;
using Neon.Communication.Packets.Outgoing.Handshake;

namespace Neon.Communication.Packets.Incoming.Handshake
{
    public class InitCryptoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new InitCryptoComposer(HabboEncryptionV2.GetRsaDiffieHellmanPrimeKey(), HabboEncryptionV2.GetRsaDiffieHellmanGeneratorKey()));
        }
    }
}