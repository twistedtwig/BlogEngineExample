using System.Text.RegularExpressions;

namespace BlogEngine.Domain.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Creates a URL friendly slug from a string
        /// </summary>
        public static string ToUrlSlug(this string str)
        {
            // Repalce any characters that are not alphanumeric with hypen
            str = Regex.Replace(str, "[^a-z^0-9]", "-", RegexOptions.IgnoreCase);

            // Replace all double hypens with single hypen
            string pattern = "--";
            while (Regex.IsMatch(str, pattern))
                str = Regex.Replace(str, pattern, "-", RegexOptions.IgnoreCase);

            // Remove leading and trailing hypens ("-")
            pattern = "^-|-$";
            str = Regex.Replace(str, pattern, "", RegexOptions.IgnoreCase);

            return str.ToLower();
        }
    }
}
