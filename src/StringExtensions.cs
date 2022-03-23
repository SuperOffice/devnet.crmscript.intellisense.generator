using System;
using System.Collections.Generic;
using System.Text;

namespace devnet.crmscript.intellisense.generator
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Surrounds string with header tags, &lt;p&gt;&lt;/p&gt;.
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static string ToParagraphTag(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return stringValue;

            return $"<p>{stringValue?.Trim()}</p>";
        }

        /// <summary>
        /// Surrounds string with header tags, &lt;h{i}&gt;&lt;/h{i}&gt;. I.e. given 1, surrounds header tag becomes H1, like &lt;h1&gt;&lt;/h1&gt;.
        /// </summary>
        /// <param name="stringValue">The string to surround.</param>
        /// <param name="headerValue">The header value.</param>
        /// <returns>If null or whitespace, returns original string, otherwise returns header tag containing the original value.</returns>
        public static string ToHeaderTag(this string stringValue, int headerValue)
        {
            if (string.IsNullOrEmpty(stringValue))
                return stringValue;

            return $"<h{headerValue}>{stringValue?.Trim()}</h{headerValue}>";
        }

        /// <summary>
        /// Surrounds string with italic tags, &lt;i&gt;&lt;/i&gt;</i>.
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static string ToItalicTag(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return stringValue;

            return $"<i>{stringValue?.Trim()}</i>";
        }
    }
}
