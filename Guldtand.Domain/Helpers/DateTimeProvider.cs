using System;
using System.Globalization;

namespace Guldtand.Domain.Helpers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public bool IsValidDate(string pid)
        {
            return DateTime.TryParseExact(pid.Substring(0, 6), "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime temp);
        }

        public DateTime Today()
        {
            return DateTime.Today;
        }
    }
}
