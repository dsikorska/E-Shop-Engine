using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace E_Shop_Engine.Website.Extensions
{
    public static class PagedListHelper
    {
        /// <summary>
        /// Before collection can be translated to paged list it needs to be sorted.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="model">Model that needs sorting.</param>
        /// <param name="defaultSortOrder">Default sort order.</param>
        /// <param name="sortOrder">Expected sort order.</param>
        /// <param name="descending">Should sorting be descending?</param>
        /// <returns>Sorted model.</returns>
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> model, Expression<Func<T, object>> defaultSortOrder, string sortOrder = null, bool descending = false)
        {
            PropertyInfo sortBy = null;

            if (!string.IsNullOrWhiteSpace(sortOrder))
            {
                sortBy = typeof(T).GetProperty(sortOrder);
            }

            IEnumerable<T> result = Enumerable.Empty<T>();

            if (sortBy == null)
            {
                result = descending ?
                    model.AsQueryable().OrderByDescending(defaultSortOrder) :
                    model.AsQueryable().OrderBy(defaultSortOrder);

                return result;
            }

            result = descending ?
                model.OrderByDescending(x => sortBy.GetValue(x, null)) :
                model.OrderBy(x => sortBy.GetValue(x, null));

            return result;
        }
    }
}
