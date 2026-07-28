using System.Globalization;
using System.Text.RegularExpressions;

namespace JamalArouna.Library.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex CamelCaseBoundary =
            new Regex("(?<!^)([A-Z])", RegexOptions.Compiled);

        public static string SplitCamelCase(this string value)
        {
            return string.IsNullOrEmpty(value)
                ? value
                : CamelCaseBoundary.Replace(value, " $1");
        }

        public static string SplitAndCapitalizeCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            string words = CamelCaseBoundary.Replace(value, " $1");
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(words.ToLowerInvariant());
        }

        public static string Capitalize(this string value)
        {
            return string.IsNullOrEmpty(value)
                ? value
                : char.ToUpperInvariant(value[0]) + value.Substring(1);
        }
    }
}
