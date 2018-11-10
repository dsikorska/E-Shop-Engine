using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using X.PagedList;

namespace E_Shop_Engine.Website.Extensions
{
    public static class PagedListHelper
    {
        public static IPagedList<TDestination> IQueryableToPagedList<TSource, TDestination, TSort>(IQueryable<TSource> model, Expression<Func<TSource, TSort>> sortCondition,
            int? page, int pageSize = 25, bool descending = false)
        {
            int pageNumber = page ?? 1;
            IPagedList<TSource> pagedModel;
            if (descending)
            {
                pagedModel = model.OrderByDescending(sortCondition).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                pagedModel = model.OrderBy(sortCondition).ToPagedList(pageNumber, pageSize);
            }
            IEnumerable<TDestination> mappedModel = Mapper.Map<IEnumerable<TDestination>>(pagedModel);
            IPagedList<TDestination> viewModel = new StaticPagedList<TDestination>(mappedModel, pagedModel.GetMetaData());
            return viewModel;
        }

        //TODO consider build expression tree: 
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries
        public static IEnumerable<TDestination> SortBy<TSource, TDestination>(IQueryable<TSource> model, string defaultSortOrder, string sortOrder = null, bool descending = false)
        {
            IQueryable<TDestination> mappedModel = model.ProjectTo<TDestination>();

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = defaultSortOrder;
            }

            PropertyInfo sortBy = typeof(TDestination).GetProperty(sortOrder);
            IEnumerable<TDestination> result = descending ? mappedModel.AsEnumerable().OrderByDescending(x => sortBy.GetValue(x, null)) :
                mappedModel.AsEnumerable().OrderBy(x => sortBy.GetValue(x, null));

            return result;
        }

        public static IEnumerable<T> SortBy<T>(IQueryable<T> model, string defaultSortOrder, string sortOrder = null, bool descending = false)
        {
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = defaultSortOrder;
            }

            PropertyInfo sortBy = typeof(T).GetProperty(sortOrder);
            IEnumerable<T> result = descending ? model.AsEnumerable().OrderByDescending(x => sortBy.GetValue(x, null)) : model.AsEnumerable().OrderBy(x => sortBy.GetValue(x, null));

            return result;
        }
    }
}
