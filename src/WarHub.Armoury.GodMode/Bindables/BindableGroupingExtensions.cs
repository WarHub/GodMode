// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Bindables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class BindableGroupingExtensions
    {
        public static IBindableGrouping<T> GroupWhere<T>(this IBindableGrouping<T> grouping, Func<T, bool> predicate)
        {
            return new BindableGrouping<T>(grouping.Where(predicate).ToArray(), grouping.Key, grouping.ShortKey);
        }

        public static IBindableGrouping<T> ToBindableGrouping<T>(this IEnumerable<T> collection, string key = null,
            string shortKey = null)
        {
            return new BindableGrouping<T>(collection, key, shortKey ?? key.ShortenKey());
        }

        public static string ShortenKey(this string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            var words = key.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words.Select(word => $"{word.Substring(0, 1)}."));
        }
    }
}
