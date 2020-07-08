using Neon.Communication.Packets.Outgoing.Help.Helpers;
using Neon.HabboHotel.GameClients;
using System.Collections.Generic;
using System.Linq;

namespace Neon.HabboHotel.Helpers
{
    public static class HelperToolsManager
    {
        public static List<HabboHelper> Helpers;
        public static List<HelperCase> Cases;
        public static int ANSWER_CALL_WAIT_TIME = 120;

        public static int GuideCount => Helpers.Count(c => c.IsGuide);

        public static int HelperCount => Helpers.Count(c => c.IsHelper);

        public static int GuardianCount => Helpers.Count(c => c.IsGuardian);



        public static void Init()
        {
            Helpers = new List<HabboHelper>();
            Cases = new List<HelperCase>();
            // Game.GetClientManager().OnClientDisconnect += HelperToolsManager_OnClientDisconnect;
        }

        public static void HelperToolsManager_OnClientDisconnect(GameClient Session)
        {
            HabboHelper client = Helpers.FirstOrDefault(c => c.Session == Session);
            if (client == null)
            { }
            IHelperElement element = GetElement(Session);
            if (element == null)
            {
                return;
            }

            if (element is HabboHelper)
            {
                HabboHelper h = (HabboHelper)element;
                RemoveHelper(h);

                if (h.Case != null)
                {
                    h.Case.End(0);
                }

                if (h.InvinteCase != null)
                {
                    h.InvinteCase.OnDecline(h);
                }
            }
            else if (element is HelperCase c)
            {
                RemoveCall(c);
                if (c.Helper != null)
                {
                    c.Helper.End(0);
                }
            }
        }

        public static HabboHelper AddHelper(GameClient Session, bool IsHelper, bool IsGard, bool IsGuide)
        {
            HabboHelper h = GetHelper(Session);
            if (h != null)
            {
                return h;
            }

            Session.GetHabbo().OnHelperDuty = true;

            h = new HabboHelper(Session, IsGuide, IsHelper, IsGard);
            Helpers.Add(h);

            return h;
        }

        public static HelperCase AddCall(GameClient Session, string message, int category)
        {
            HelperCase c = GetCall(Session);
            if (c != null)
            {
                return c;
            }

            HelperCase hcase = new HelperCase(Session, message, category);
            Cases.Add(hcase);
            return hcase;
        }

        public static HabboHelper GetHelper(GameClient Session)
        {
            return Helpers.FirstOrDefault(c => c.Session == Session);
        }


        public static void RemoveHelper(HabboHelper client)
        {
            Helpers.Remove(client);
        }

        public static void RemoveCall(HelperCase Call)
        {
            Cases.Remove(Call);
        }

        public static void RemoveCall(GameClient client)
        {
            HelperCase call = GetCall(client);
            if (call != null)
            {
                RemoveCall(call);
            }
        }


        public static void RemoveHelper(GameClient Session)
        {
            HabboHelper h = GetHelper(Session);
            if (h != null)
            {
                RemoveHelper(h);
            }

            foreach (HabboHelper helper in Helpers)
            {
                if (helper.Session.GetHabbo() == null)
                {
                    RemoveHelper(helper);
                }
            }
        }

        public static HelperCase GetCall(GameClient Session)
        {
            return Cases.FirstOrDefault(c => c.Session == Session);
        }

        public static void InvinteHelpCall(HabboHelper Helper, HelperCase hcase)
        {
            Helper.InvinteCase = hcase;
            Helper.Session.SendMessage(new CallForHelperWindowComposer(true, hcase));
            hcase.Helper = Helper;
        }

        public static IHelperElement GetElement(GameClient Session)
        {
            return Cases.Union<IHelperElement>(Helpers).FirstOrDefault(c => c.Session == Session);
        }


        public static List<HabboHelper> GetAvaliableHelpers()
        {
            return Helpers.Where(c => !c.Busy).ToList();
        }

        public static List<HabboHelper> GetHelpersToCase(HelperCase Case)
        {
            return GetAvaliableHelpers().Where(c => !Case.DeclinedHelpers.Any(d => d == c)).Where(c => Case.Session != c.Session && ((c.IsGuide && Case.Type == HelpCaseType.MEET_HOTEL) || (c.IsHelper && Case.Type == HelpCaseType.INSTRUCTION))).ToList();
        }



    }
}
