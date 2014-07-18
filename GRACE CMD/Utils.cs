using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRACE_CMD
{
    public static class Utils
    {
        public static double coerce(double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
        public static DateTime GetTime(double secondsJuliet)
        {
            DateTime time = new DateTime(0, DateTimeKind.Utc);
            time = time.AddYears(1999);
            time = time.AddSeconds(secondsJuliet);
            return time;
        }
    }
}
