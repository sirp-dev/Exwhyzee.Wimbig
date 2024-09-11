using System;

namespace Exwhyzee
{
    public static class StringExtensions
    {
        public static string EnsureMaximumLength(this string value, int maximumLength)
        {
            if (maximumLength <= 0)
                throw new Exception($"{nameof(maximumLength)} must be greater than 0.");

            if (string.IsNullOrEmpty(value))
                return value;

            if (value.Length <= maximumLength)
                return value;

            return value.Substring(0, maximumLength);
        }

        public static string TrimSafe(this string value)
        {
            return value?.Trim();
        }

        public static bool IsNumeric(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            double number = 0;

            return double.TryParse(value, out number);
        }

        /// <summary>
        /// Converts a <see cref="System.String"/> object to null if it's original value is an empty string.
        /// Returns the original value if it is not an empty string.
        /// </summary>
        /// <param name="value">string to convert</param>
        /// <returns>string</returns>
        public static string MakeNullIfEmpty(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? value : null;
        }
    }
}
