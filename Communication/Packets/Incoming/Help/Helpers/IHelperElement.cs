
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Helpers
{
    public interface IHelperElement
    {
        GameClient Session { get; set; }
        IHelperElement OtherElement { get; }
        void End(int ErrorCode = 1);
        void Close();
        //void SendMessage(string message);

    }
}
