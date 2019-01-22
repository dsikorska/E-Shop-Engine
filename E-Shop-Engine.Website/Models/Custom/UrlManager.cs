using System.Collections.Generic;
using System.Linq;

namespace E_Shop_Engine.Website.Models.Custom
{
    public static class UrlManager
    {
        /// <summary>
        /// Use only to reset sorting/paging.
        /// </summary>
        public static string PreviousUrl { get; private set; }

        private static Stack<string> Urls = new Stack<string>();

        public static bool IsReturning { get; set; }

        /// <summary>
        /// Use only to reset sorting/paging.
        /// </summary>
        public static void SetPreviousUrl(string url)
        {
            PreviousUrl = url;
        }

        public static void AddUrl(string url)
        {
            Urls.Push(url);
        }

        public static string PopUrl()
        {
            if (Urls.Count != 0)
            {
                return Urls.Pop();
            }
            return "/";
        }

        public static void ClearStack()
        {
            Urls.Clear();
        }

        public static bool IsAtIndexView(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                return url.Where(s => s == '/').Count() == 1 || url.IndexOf("Index") != -1;
            }

            return true;
        }

        public static bool IsLastView(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                return url.IndexOf("Edit") != -1 && url.IndexOf("Create") != -1;
            }

            return false;
        }
    }
}
