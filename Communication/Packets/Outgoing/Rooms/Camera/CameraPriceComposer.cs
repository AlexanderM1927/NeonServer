namespace Neon.Communication.Packets.Outgoing.Rooms.Camera
{
    public class CameraPriceComposer : ServerPacket
    {
        public CameraPriceComposer(int Credits, int Duckets, int Unknown)
            : base(ServerPacketHeader.CameraPriceComposer)
        {
            base.WriteInteger(Credits);
            base.WriteInteger(Duckets);
            base.WriteInteger(Unknown);
        }
    }
}