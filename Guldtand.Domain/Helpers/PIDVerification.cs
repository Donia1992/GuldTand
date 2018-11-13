using System.Linq;
using System.Text.RegularExpressions;

namespace Guldtand.Domain.Helpers
{
    public static class PIDVerification
    {
        public static bool LuhnCheck(this string pid)
        {
            string pidString = Regex.Replace(pid, @"\s|\-", "");

            if (pidString.Length == 12) { pidString = pidString.Substring(2); }
            if (!pidString.All(char.IsNumber)) { return false; }
            if (pidString.Length != 10) { return false; }

            return LuhnCheck(pid.Select(c => c - '0').ToArray());
        }

        private static bool LuhnCheck(this int[] digits)
        {
            return GetCheckValue(digits) == 0;
        }

        private static int GetCheckValue(int[] digits)
        {
            return digits.Select((d, i) => i % 2 == digits.Length % 2 ? ((2 * d) % 10) + d / 5 : d).Sum() % 10;
        }

    }
}
