﻿using System;

namespace Neon.HabboHotel.Users.Authenticator
{
    [Serializable]
    public class IncorrectLoginException : Exception
    {
        public IncorrectLoginException(string Reason) : base(Reason)
        {
        }
    }
}