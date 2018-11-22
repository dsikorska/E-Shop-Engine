namespace E_Shop_Engine.Website.Models.Custom
{
    public static class NotifyManager
    {
        public static string Type { get; private set; }
        public static string Title { get; private set; }
        public static string Text { get; private set; }

        public static void Set(string type, string title, string text)
        {
            Type = type;
            Title = title;
            Text = text;
        }
    }
}