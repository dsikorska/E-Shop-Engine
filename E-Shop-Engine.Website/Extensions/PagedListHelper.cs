using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace E_Shop_Engine.Website.Extensions
{
    public static class PagedListHelper
    {
        public static IEnumerable<T> SortBy<T, TKey>(IEnumerable<T> model, Expression<Func<T, TKey>> defaultSortOrder, string sortOrder = null, bool descending = false)
        {
            PropertyInfo sortBy = null;

            if (sortOrder != null)
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
