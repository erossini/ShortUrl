using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.ExtensionTypes
{
    public static class StringExtensions
    {
        /// <summary>
        /// Extract a domain name from a full URL
        /// </summary>
        /// <param name="Url">Full URL</param>
        /// <returns>Domain</returns>
        public static string ExtractDomainNameFromURL(this string Url)
        {
            string rtn = "";
            if (!string.IsNullOrEmpty(Url))
            {
                rtn = System.Text.RegularExpressions.Regex.Replace(
                            Url,
                            @"^([a-zA-Z]+:\/\/)?([^\/]+)\/.*?$",
                            "$2"
                        );
            }

            return rtn;
        }

        /// <summary>
        /// Gets a number from a IPv4
        /// </summary>
        /// <param name="Ip">IPv4 to convert in a number</param>
        /// <returns></returns>
        public static decimal IPToNumber(this string Ip)
        {
            decimal ipnum = 0;

            // verifiy the IP is an IPv4 
            // IPv6 has :
            if (Ip.IndexOf(":") < 0)
            {
                string[] split = Ip.Split('.');
                ipnum = Convert.ToInt64(split[0]) * (256 * 256 * 256) + Convert.ToInt64(split[1]) * (256 * 256) +
                        Convert.ToInt64(split[2]) * 256 + Convert.ToInt64(split[3]);
            }

            return ipnum;
        }

        public static string PadNumber(this string text)
        {
            return text.PadRight(1, '0');
        }

        /// <summary>
        /// Returns a random string with random alphanumeric characters
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">String length</param>
        /// <returns>A string with random alphanumeric characters</returns>
        public static string RandomString(this String str, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Takes a substring between two anchor strings (or the end of the string if that anchor is null)
        /// </summary>
        /// <param name="this">a string</param>
        /// <param name="from">an optional string to search after</param>
        /// <param name="until">an optional string to search before</param>
        /// <param name="comparison">an optional comparison for the search</param>
        /// <returns>a substring based on the search</returns>
        public static string SubstringBetween(this string @this, string from = null, string until = null, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var fromLength = (from ?? string.Empty).Length;
            var startIndex = !string.IsNullOrEmpty(from) ? @this.IndexOf(from, comparison) + fromLength : 0;

            if (startIndex < fromLength)
            {
                throw new ArgumentException("from: Failed to find an instance of the first anchor");
            }

            var endIndex = !string.IsNullOrEmpty(until) ? @this.IndexOf(until, startIndex, comparison) : @this.Length;

            if (endIndex < 0)
            {
                endIndex = @this.Length;
            }

            var subString = @this.Substring(startIndex, endIndex - startIndex);
            return subString;
        }

        /// <summary>
        /// Truncate a string after maxLength characters.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns>A string truncates after maxLength characters. If the string length is less the maxLength, return the string</returns>
        public static string TruncateString(this string str, int maxLength)
        {
            return new string(str.Take(maxLength).ToArray());
        }
    }
}