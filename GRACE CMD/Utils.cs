using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRACE_CMD
{
    /// <summary>
    /// Utilities class
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Coerce a value to remain within a min and max
        /// </summary>
        /// <param name="value">Value to coerce</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Coerced value as double</returns>
        public static double coerce(double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
        /// <summary>
        /// Get a DateTime from seconds past year 2000
        /// </summary>
        /// <param name="secondsJuliet">Seconds past midnight Jan 1, 2000 UTC</param>
        /// <returns>DateTime in UTC format</returns>
        public static DateTime GetTime(double secondsJuliet)
        {
            DateTime time = new DateTime(0, DateTimeKind.Utc);
            time = time.AddYears(1999);
            time = time.AddSeconds(secondsJuliet);
            return time;
        }
    }
}
