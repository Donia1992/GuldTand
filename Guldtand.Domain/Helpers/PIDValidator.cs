using System.Linq;

namespace Guldtand.Domain.Helpers
{
    public class PIDValidator : IPIDValidator
    {
        public bool Validate(string pid)
        {
            return LuhnCheck(pid.Select(c => c - '0').ToArray());
        }

        private static bool LuhnCheck(int[] digits)
        {
            return GetCheckValue(digits) == 0;
        }

        private static int GetCheckValue(int[] digits) => 
            digits.Select((d, i) => i % 2 == digits.Length % 2 ? ((2 * d) % 10) + d / 5 : d).Sum() % 10;
    }
}
