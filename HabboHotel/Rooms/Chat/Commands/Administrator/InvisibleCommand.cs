namespace Neon.HabboHotel.Rooms.Chat.Commands.Staff
{
    internal class InvisibleCommand : IChatCommand
    {
        public string PermissionRequired => "command_invisible";

        public string Parameters => "";

        public string Description => "Use this command to turn on/off invisible mode";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Session == null)
            {
                return;
            }

            if (Session.GetHabbo().BlnInv)
            {
                Session.GetHabbo().BlnInv = false;
                Session.SendWhisper("Invisible Mode OFF", 4);
            }
            else
            {
                Session.GetHabbo().BlnInv = true;
                Session.SendWhisper("Invisible Mode ON -- Only Activated for Next Room Visit", 4);
            }
        }
    }
}