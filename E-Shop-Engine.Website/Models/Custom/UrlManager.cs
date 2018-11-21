using System.Collections.Generic;

namespace E_Shop_Engine.Website.Models.Custom
{
    public static class UrlManager
    {
        public static string PreviousUrl { get; private set; }

        private static Stack<string> Urls = new Stack<string>();

        public static void SetPreviousUrl(string url)
        {
            PreviousUrl = url;
        }

        public static void AddUrl(string url)
        {
            Urls.Push(url);
        }

        public static string GetLastUrl()
        {
            return Urls.Peek();
        }

        public static void ClearStack()
        {
            Urls.Clear();
        }

        public static string GetUrl(int stage)
        {
            for (int i = 0; i < stage; i++)
            {
                if (i == stage - 1)
                {
                    return Urls.Peek();
                }

                Urls.Pop();
            }

            return null;
        }
    }
}
