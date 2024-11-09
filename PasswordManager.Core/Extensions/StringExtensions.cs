using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordManager.Core.Extensions
{
    public static partial class StringExtensions
    {
        public const string CarriageReturnLineFeed = "\r\n";
        public const string Empty = "";
        public const char CarriageReturn = '\r';
        public const char LineFeed = '\n';
        public const char Tab = '\t';

        private delegate void ActionLine(TextWriter textWriter, string line);

        [DebuggerStepThrough]
        public static string ToSafe(this string value, string defaultValue = null)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            return (defaultValue ?? string.Empty);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TrimSafe(this string value)
        {
            return (!string.IsNullOrEmpty(value) ? value.Trim() : value);
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string EmptyNull(this string value)
        {
            return (value ?? string.Empty).Trim();
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullEmpty(this string value)
        {
            return (string.IsNullOrEmpty(value)) ? null : value;
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dump(this string value, bool appendMarks = false)
        {
            Debug.WriteLine(value);
            Debug.WriteLineIf(appendMarks, "------------------------------------------------");
        }

        /// <summary>
        /// Returns n/a if string is empty else self.
        /// </summary>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NaIfEmpty(this string value)
        {
            return (string.IsNullOrWhiteSpace(value) ? "n/a" : value);
        }

        /// <summary>
        /// Removes Diacritics From a String
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString.EnumerateRunes())
            {
                var unicodeCategory = Rune.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// A nicer way of calling <see cref="System.String.IsNullOrEmpty(string)"/>
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// A nicer way of calling the inverse of <see cref="System.String.IsNullOrEmpty(string)"/>
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is not null or an empty string (""); otherwise, false.</returns>
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !value.IsNullOrEmpty();
        }

        /// <summary>
        /// Slugifies a string
        /// </summary>
        /// <param name="value">The string value to slugify</param>
        /// <param name="maxLength">An optional maximum length of the generated slug</param>
        /// <returns>A URL safe slug representation of the input <paramref name="value"/>.</returns>
        public static string ToSlug(this string value, int? maxLength = null)
        {
            // Ensure.Argument.NotNull(value, "value");

            // if it's already a valid slug, return it
            if (RegexUtils.SlugRegex.IsMatch(value))
                return value;

            return GenerateSlug(value, maxLength);
        }

        /// <summary>
        /// Credit for this method goes to http://stackoverflow.com/questions/2920744/url-slugify-alrogithm-in-cs
        /// </summary>
        private static string GenerateSlug(string value, int? maxLength = null)
        {
            // prepare string, remove accents, lower case and convert hyphens to whitespace
            var result = RemoveAccent(value).Replace("-", " ").ToLowerInvariant();

            result = Regex.Replace(result, @"[^a-z0-9\s-]", string.Empty); // remove invalid characters
            result = Regex.Replace(result, @"\s+", " ").Trim(); // convert multiple spaces into one space

            if (maxLength.HasValue) // cut and trim
                result = result.Substring(0, result.Length <= maxLength ? result.Length : maxLength.Value).Trim();

            return Regex.Replace(result, @"\s", "-"); // replace all spaces with hyphens
        }

        private static string RemoveAccent(string value)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string FirstCharToUpper(this string input) =>
              input switch
              {
                  null => throw new ArgumentNullException(nameof(input)),
                  "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                  _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
              };
    }
}
