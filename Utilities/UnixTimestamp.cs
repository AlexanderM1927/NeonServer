﻿using System;

namespace Neon.Utilities
{
    internal static class UnixTimestamp
    {
        /// <summary>
        /// Gets the current date time now in Unix Timestamp format.
        /// </summary>
        /// <returns>Unix Timestamp.</returns>
        public static double GetNow()
        {
            TimeSpan ts = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0));
            return ts.TotalSeconds;
        }

        /// <summary>
        /// Converts the Unix Timestamp to a DateTime object.
        /// </summary>
        /// <param name="Timestamp">Unix Timestamp.</param>
        /// <returns>DateTime object.</returns>
        public static DateTime FromUnixTimestamp(double Timestamp)
        {
            DateTime DT = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DT = DT.AddSeconds(Timestamp);
            return DT;
        }
    }
}
