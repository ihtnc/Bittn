using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bittn.Api
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> items, string sortField, bool? sortDescending)
            where T : class
        {
            var prop = typeof(T)
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .SingleOrDefault(p => p.Name.Equals(sortField, StringComparison.OrdinalIgnoreCase));

            var needSort = prop != null && !string.IsNullOrWhiteSpace(sortField);
            if(needSort && sortDescending == true)
            {
                return items.OrderByDescending(item => prop.GetValue(item));
            }
            else if(needSort && sortDescending != true)
            {
                return items.OrderBy(item => prop.GetValue(item));
            }

            return items;
        }

        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> items, int itemsPerPage, int? currentPageIndex)
            where T : class
        {
            var limit = itemsPerPage;
            var page = currentPageIndex < 0 ? 0 : currentPageIndex ?? 0;
            var maxItems = limit + (limit * page);
            return items.Skip(maxItems - limit).Take(limit);
        }
    }
}