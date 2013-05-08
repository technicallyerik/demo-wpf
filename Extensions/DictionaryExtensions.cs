using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Collections.Generic
{
    /// <summary>
    /// These are extension methods for use on dictionaries.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Converts a dictionary of key value pairs to a URL encoded parameter string.
        /// </summary>
        /// <param name="nvc">The dictionary of key value pairs.</param>
        /// <returns></returns>
        public static string ToQueryString(this Dictionary<string, string> nvc)
        {
            return "?" +
                   string.Join("&",
                               nvc.Select(
                                   kvp =>
                                   string.Format("{0}={1}", HttpUtility.UrlEncode(kvp.Key),
                                                 HttpUtility.UrlEncode(kvp.Value))));
        }
    }
}