using System;

namespace Liquid.Core.Utils
{
    /// <summary>
    /// Number Extensions Class.
    /// </summary>
    public static class NumberUtils
    {
        /// <summary>
        /// Determines whether a number is is prime number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>
        ///   <c>true</c> if [is prime number] [the specified number]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrimeNumber(this int number)
        {
            if (number % 2 == 0)
            {
                return number == 2;
            }
            var sqrt = (int)Math.Sqrt(number);
            for (var t = 3; t <= sqrt; t += 2)
            {
                if (number % t == 0)
                {
                    return false;
                }
            }
            return number != 1;
        }

        /// <summary>
        /// Determines whether a number is odd.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>
        ///   <c>true</c> if the specified number is odd; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOdd(this int number)
        {
            return number % 2 == 0;
        }

        /// <summary>
        /// Determines whether a number is even.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>
        ///   <c>true</c> if the specified number is even; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEven(this int number)
        {
            return number % 2 != 0;
        }

        /// <summary>
        /// Format a double using the local culture currency settings.
        /// </summary>
        /// <param name="value">The double to be formatted.</param>
        /// <returns>The double formatted based on the local culture currency settings.</returns>
        public static string ToLocalCurrencyString(this double value)
        {
            return $"{value:C}";
        }
    }
}