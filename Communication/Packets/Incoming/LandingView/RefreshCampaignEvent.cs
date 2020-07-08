using Neon.Communication.Packets.Outgoing.LandingView;

namespace Neon.Communication.Packets.Incoming.LandingView
{
    internal class RefreshCampaignEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            try
            {
                string parseCampaings = Packet.PopString();
                if (parseCampaings == "2015-08-18 13:00,gamesmaker;2015-08-19 13:00")
                {
                    Session.SendMessage(new HallOfFameComposer());
                    return;
                }

                string campaingName = "";
                string[] parser = parseCampaings.Split(';');

                for (int i = 0; i < parser.Length; i++)
                {
                    if (string.IsNullOrEmpty(parser[i]) || parser[i].EndsWith(","))
                    {
                        continue;
                    }

                    string[] data = parser[i].Split(',');
                    campaingName = data[1];
                }
                Session.SendMessage(new CampaignComposer(parseCampaings, campaingName));

                Session.SendMessage(new LimitedCountdownExtendedComposer());

                if (campaingName.Contains("CommunityGoal"))
                {
                    Session.SendMessage(new CommunityGoalComposer());
                    Session.SendMessage(new DynamicPollLandingComposer(false)); // Si este campo está en false el usuario puede votar.
                }
            }
            catch { }
        }
    }
}