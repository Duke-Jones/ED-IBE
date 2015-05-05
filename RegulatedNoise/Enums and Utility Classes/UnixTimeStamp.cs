using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    static class UnixTimeStamp
    {
        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToUniversalTime();
            return dtDateTime;
        }

        public static int  DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (Int32)(dateTime - new DateTime(1970, 1, 1).ToUniversalTime()).TotalSeconds;
        }
    }
}
