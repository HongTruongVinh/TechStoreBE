using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Common.Helpers
{
    public class TimeZoneHelper
    {
        private static readonly TimeZoneInfo Gmt7TimeZone = TimeZoneInfo.FindSystemTimeZoneById(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "SE Asia Standard Time" : "Asia/Ho_Chi_Minh");

        public static DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        public static DateTime ConvertUtcToGmt7(DateTime utcDateTime)
        {
            if (utcDateTime.Kind == DateTimeKind.Unspecified)
            {
                // Nếu không rõ Kind, giả định là UTC
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, Gmt7TimeZone);
        }

        public static DateTime GetGmt7Now()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Gmt7TimeZone);
        }

        public static DateTime ConvertGmt7ToUtc(DateTime? gmt7DateTime)
        {
            if (gmt7DateTime == null)
                return DateTime.SpecifyKind(new DateTime(1900, 1, 1), DateTimeKind.Utc); ;

            return TimeZoneInfo.ConvertTimeToUtc(gmt7DateTime.Value, Gmt7TimeZone);
        }
    }
}
