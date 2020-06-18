using System;

namespace Neon.HabboHotel.Users.UserDataManagement
{
    public class UserDataNotFoundException : Exception
    {
        public UserDataNotFoundException(string reason)
            : base(reason)
        {
        }
    }
}