﻿using Neon.Communication.Packets.Outgoing.Help.Helpers;
using Neon.HabboHotel.GameClients;

namespace Neon.HabboHotel.Helpers
{
    public class HabboHelper : IHelperElement
    {
        public GameClient Session { get; set; }
        public bool IsGuardian, IsHelper, IsGuide;
        public HelperCase Case;
        public HelperCase InvinteCase;

        public bool Busy => Case != null || InvinteCase != null;

        public IHelperElement OtherElement => Case;

        public HabboHelper(GameClient Session, bool guide, bool helper, bool guard)
        {
            this.Session = Session;
            IsGuide = guide;
            IsGuardian = guard;
            IsHelper = helper;
        }

        public void Accept()
        {
            if (InvinteCase == null)
            {
                Session.SendMessage(new CloseHelperSessionComposer());
                return;
            }
            Case = InvinteCase;
            InvinteCase = null;


            Session.SendMessage(new InitHelperSessionChatComposer(Case.Session.GetHabbo(), Session.GetHabbo()));
            Case.Session.SendMessage(new InitHelperSessionChatComposer(Case.Session.GetHabbo(), Session.GetHabbo()));
        }

        public void Decline()
        {
            InvinteCase.OnDecline(this);
            InvinteCase = null;
            Case = null;
            Session.SendMessage(new CloseHelperSessionComposer());
        }

        public void CancelCall()
        {
            if (InvinteCase != null)
            {
                InvinteCase.Session.SendMessage(new CloseHelperSessionComposer());
            }

            InvinteCase = null;
            if (Case != null)
            {
                Case.Session.SendMessage(new CloseHelperSessionComposer());
            }

            Case = null;
            Session.SendMessage(new CloseHelperSessionComposer());
        }

        public void End(int ErrorCode = 1)
        {
            Session.SendMessage(new EndHelperSessionComposer(ErrorCode));
        }

        public void Close()
        {
            Case = null;
            InvinteCase = null;
            Session.SendMessage(new CloseHelperSessionComposer());
        }
    }
}
