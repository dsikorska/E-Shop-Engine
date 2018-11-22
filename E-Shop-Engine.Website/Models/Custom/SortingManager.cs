namespace E_Shop_Engine.Website.Models.Custom
{
    public static class SortingManager
    {
        public static string SortOrder { get; private set; }
        public static bool IsSortDescending { get; private set; }
        public static string SearchTerm { get; private set; }

        public static void SetSorting(string order, bool descending, string search = "")
        {
            SortOrder = order;
            IsSortDescending = descending;
            SearchTerm = search;
        }

        public static void SetSearchingTerm(string text)
        {
            SearchTerm = text;
        }

        public static void ResetSorting()
        {
            SortOrder = null;
            SearchTerm = null;
            IsSortDescending = false;
        }
    }
}