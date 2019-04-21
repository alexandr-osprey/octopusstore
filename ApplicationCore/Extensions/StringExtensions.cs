using System;

namespace ApplicationCore.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Case insensitive string Contains
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        public static bool ContainsCI(this string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Case insentive string Equals
        /// </summary>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsCI(this string source, string other)
        {
            return string.Compare(source, other, true) == 0;
        }
    }
}