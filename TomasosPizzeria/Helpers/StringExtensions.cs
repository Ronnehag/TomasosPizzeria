using System.Linq;

namespace TomasosPizzeria.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the first letter in the string to uppercase
        /// </summary>
        public static string ToFirstLetterUpper(this string str)
        {
            return str.ToLower()
                       .First()
                       .ToString()
                       .ToUpper() + str.Substring(1);
        }
    }
}
