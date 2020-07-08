
using Neon.HabboHotel.Users.Effects;

namespace Neon.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
    internal class AvatarEffectExpiredComposer : ServerPacket
    {
        public AvatarEffectExpiredComposer(AvatarEffect Effect)
            : base(ServerPacketHeader.AvatarEffectExpiredMessageComposer)
        {
            base.WriteInteger(Effect.SpriteId);
        }
    }
}
