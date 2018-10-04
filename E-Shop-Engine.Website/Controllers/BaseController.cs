using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using X.PagedList;

namespace E_Shop_Engine.Website.Controllers
{
    public class BaseController : Controller
    {
        protected IPagedList<T> IEnumerableToPagedList<T>(IEnumerable<T> model, int? page, int pageSize = 25)
        {
            int pageNumber = page ?? 1;
            IPagedList<T> pagedModel = new PagedList<T>(model, pageNumber, pageSize);
            return pagedModel;
        }

        protected IPagedList<TDestination> IEnumerableToPagedList<TSource, TDestination, TSort>(IEnumerable<TSource> model, Expression<Func<TSource, TSort>> sortCondition, int? page, int pageSize = 25, bool descending = false)
        {
            int pageNumber = page ?? 1;
            IPagedList<TSource> pagedModel;
            if (descending)
            {
                pagedModel = model.AsQueryable().OrderByDescending(sortCondition).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                pagedModel = model.AsQueryable().OrderBy(sortCondition).ToPagedList(pageNumber, pageSize);
            }
            IEnumerable<TDestination> mappedModel = Mapper.Map<IEnumerable<TDestination>>(pagedModel);
            IPagedList<TDestination> viewModel = new StaticPagedList<TDestination>(mappedModel, pagedModel.GetMetaData());
            return viewModel;
        }

        protected IPagedList<TDestination> IQueryableToPagedList<TSource, TDestination, TSort>(IQueryable<TSource> model, Expression<Func<TSource, TSort>> sortCondition, int? page, int pageSize = 25, bool descending = false)
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
        protected IEnumerable<TDestination> SortBy<TSource, TDestination>(IQueryable<TSource> model, string defaultSortOrder, string sortOrder = null, bool descending = false)
        {
            IQueryable<TDestination> mappedModel = model.ProjectTo<TDestination>();

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = defaultSortOrder;
            }

            PropertyInfo sortBy = typeof(TDestination).GetProperty(sortOrder);
            IEnumerable<TDestination> result = descending ? mappedModel.AsEnumerable().OrderByDescending(x => sortBy.GetValue(x, null)) : mappedModel.AsEnumerable().OrderBy(x => sortBy.GetValue(x, null));

            return result;
        }

        protected void ReverseSorting(ref bool descending, string sortOrder)
        {
            if (TempData.ContainsKey("SortOrder") &&
                TempData["SortOrder"] != null &&
                sortOrder == TempData["SortOrder"].ToString() &&
                descending == (bool)TempData["SortDescending"])
            {
                descending = !descending;
            }
        }

        protected void SaveSortingState(string sortOrder, bool descending)
        {
            TempData["SortOrder"] = sortOrder;
            TempData["SortDescending"] = descending;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortDescending = descending;
        }
    }
}