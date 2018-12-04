using System.Collections.Generic;

namespace E_Shop_Engine.Website.Models.Custom
{
    public static class UrlManager
    {
        /// <summary>
        /// Use only for paging/sorting!
        /// </summary>
        public static string PreviousUrl { get; private set; }

        private static Stack<string> Urls = new Stack<string>();

        public static bool IsReturning { get; set; }

        /// <summary>
        /// Use only for paging/sorting!
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
    }
}
